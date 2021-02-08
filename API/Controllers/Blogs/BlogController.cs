using API.ErrorsHandlers;
using AutoMapper;
using Core.Dtos.Blogs;
using Core.Helppers;
using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Models.Identity;
using Core.Models.Uploads;
using Core.Specifications.Blogs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Blogs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Blog> _blogRepo;
        private readonly IGenericRepository<UploadBlogImagesList> _uploadImageRepo;
        private readonly IGenericRepository<BlogComment> _commentRepo;
        private readonly IGenericRepository<BlogLike> _likeRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BlogController(UserManager<AppUser> userManager,
                                IGenericRepository<Blog> blogRepo,
                                IGenericRepository<UploadBlogImagesList> uploadImageRepo,
                                IGenericRepository<BlogComment> commentRepo,
                                IGenericRepository<BlogLike> likeRepo,
                                IMapper mapper,
                                AppDbContext context
                                )
        {
            _userManager = userManager;
            _blogRepo = blogRepo;
            _uploadImageRepo = uploadImageRepo;
            _commentRepo = commentRepo;
            _likeRepo = likeRepo;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetAllBlogCardList")]
        public async Task<ActionResult<Pagination<BlogCardDto>>> GetAllBlog([FromForm] SpecificParameters par)
        {
            var user = await GetCurrentUserAsync(HttpContext.User);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var userLangs = await _context.UserSelectedLanguages.Where(l => l.UserId == user.Id).Select(l=>l.LanguageId).ToListAsync();


            // this to get blogs by CategoryId, one to many relationships between Blog and BlogCategoriesList Tables. blog has one or more categories. FOR TEST
            //var blogs = await _context.Blog.Include(u=>u.UploadBlogImagesList).Where(b => b.BlogCategoriesList.OrderByDescending(c => c.Id).First().BlogCategoryId == par.CategoryId &&
            //                                            userLangs.Contains(b.LanguageId)) .ToListAsync();
            //var blogCount = blogs.Count();
            //**********************************

            var spec = new GetBlogsListPaginationOrBlogDetailsSpeci(par, userLangs);
            var blogs = await _blogRepo.ListAsync(spec);

            // to get the count, it's the same criteria if spec
            var blogCountSpec = new GetBlogsListPaginationOrBlogDetailsSpeci(par, userLangs, emptyConstructor:true);
            int blogsCount = await _blogRepo.CountAsync(blogCountSpec);

            var _blogs = _mapper.Map<IReadOnlyList<Blog>, IReadOnlyList< BlogCardDto>> (blogs);

            foreach (var blog in _blogs)
            {
                // add default image in card
                var imagesList = await _uploadImageRepo.ModelDetailsAsync(new getBlogImagesListOrDefaultImageSpeci(blog.Id));
                blog.DefaultBlogImage = imagesList.Path;

                // count the comments
                blog.CommentsCount = await _commentRepo.CountAsync(new GetBlogCommentsSpeci(blog.Id));
                //count the like
                blog.LikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:true));
                //count the dislike
                blog.DislikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:false));
            }

            return new Pagination<BlogCardDto>(par.PageIndex, par.PageSize, blogsCount, _blogs);
        }

        [HttpGet("GetBlogDetails")]
        public async Task<ActionResult<BlogDto>> GetBlogDetails([FromForm] int id)
        {
            
            var spec = new GetBlogsListPaginationOrBlogDetailsSpeci(id);

            var blog = await _blogRepo.ModelDetailsAsync(spec);
            var _blog = _mapper.Map<Blog, BlogDto>(blog);
            // count the comments
            _blog.CommentsCount = await _commentRepo.CountAsync(new GetBlogCommentsSpeci(blog.Id));
            //count the like
            _blog.LikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:true));
            //count the dislike
            _blog.DislikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:false));

            // add all comment for this blog
            var comments= await _commentRepo.ListAsync(new GetBlogCommentsSpeci(_blog.Id));
            _blog.BlogComments = _mapper.Map<IReadOnlyList<BlogComment>, IReadOnlyList<BlogCommentDto>>(comments);

            // get Blogs images;
            IReadOnlyList<UploadBlogImagesList> imagesList = await _uploadImageRepo.ListAsync(new getBlogImagesListOrDefaultImageSpeci(_blog.Id, defaultImg: true)) ;
            _blog.BlogImagesList = _mapper.Map<IReadOnlyList<UploadBlogImagesList>, IReadOnlyList<BlogImageDto>>(imagesList);

            return Ok(_blog);
        }


        /// <summary>
        /// Inside WebToken there is an Email, from this email Get User from user associated on controller and HttpContext absract class.
        /// </summary>
        /// <returns>
        /// If ClaimsPrincipal httpContextUser = HttpContext.User; retrun an User Object 
        /// else retrun null</returns> 
        private async Task<AppUser> GetCurrentUserAsync(ClaimsPrincipal httpContextUser)
        {
            if (httpContextUser != null)
            {
                // get an email form Token Claim, that has been added in TockenServices.cs
                var email = httpContextUser?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var user = await _userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
