﻿using API.ErrorsHandlers;
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
using Microsoft.AspNetCore.Hosting;
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
        private const string BLOG_IMAGE_DIRECTORY = "/Uploads/Images/";

        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Blog> _blogRepo;
        private readonly IGenericRepository<UploadBlogImagesList> _uploadImageRepo;
        private readonly IGenericRepository<BlogComment> _commentRepo;
        private readonly IGenericRepository<BlogLike> _likeRepo;
        private readonly IGenericRepository<BlogSourceCategoryName> _blogSourceCategoryRepo;
        private readonly IGenericRepository<BlogCategoryList> _blogCategoryListRepo;
        private readonly IGenericRepository<BlogCategory> _blogCategoryRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(UserManager<AppUser> userManager,
                                IGenericRepository<Blog> blogRepo,
                                IGenericRepository<UploadBlogImagesList> uploadImageRepo,
                                IGenericRepository<BlogComment> commentRepo,
                                IGenericRepository<BlogLike> likeRepo,
                                IGenericRepository<BlogSourceCategoryName> blogSourceCategoryRepo,
                                IGenericRepository<BlogCategoryList> blogCategoryListRepo,
                                IGenericRepository<BlogCategory> blogCategoryRepo,
                                IMapper mapper,
                                AppDbContext context,
                                IWebHostEnvironment webHostEnvironment
                                )
        {
            _userManager = userManager;
            _blogRepo = blogRepo;
            _uploadImageRepo = uploadImageRepo;
            _commentRepo = commentRepo;
            _likeRepo = likeRepo;
            _blogSourceCategoryRepo = blogSourceCategoryRepo;
            _blogCategoryListRepo = blogCategoryListRepo;
            _blogCategoryRepo = blogCategoryRepo;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            // query all blogs with pagination tools
            IReadOnlyList<Blog> blogs = await _blogRepo.ListAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(par, userLangs));
            // to get the count, it's the same criteria if spec
            int blogsCount = await _blogRepo.CountAsync( new GetBlogsListPaginationOrBlogDetailsSpeci(par, userLangs, emptyConstructor:true));

            // mapping from Blog -> BlogCardDto
            IReadOnlyList<BlogCardDto> _blogs = _mapper.Map<IReadOnlyList<Blog>, IReadOnlyList< BlogCardDto>> (blogs);
            
            // looping to add some data
            foreach (var blog in _blogs)
            {
                // add default image in BlogCard
                UploadBlogImagesList imagesList = await _uploadImageRepo.ModelDetailsAsync(new getBlogImagesListOrDefaultImageSpeci(blog.Id));
                if(imagesList != null )
                    blog.DefaultBlogImage = imagesList.Path;

                // count Blog's comments
                blog.CommentsCount = await _commentRepo.CountAsync(new GetBlogCommentsSpeci(blog.Id));
                //count Blog's Like
                blog.LikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:true));
                //count Blog's Dislike
                blog.DislikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:false));
            }

            // return using pagination tools
            return new Pagination<BlogCardDto>(par.PageIndex, par.PageSize, blogsCount, _blogs);
        }

        [HttpGet("GetBlogDetails")]
        public async Task<ActionResult<BlogDto>> GetBlogDetails([FromForm] int id)
        {
            // get Blog Details
            var blog = await _blogRepo.ModelDetailsAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(id));
            if(blog == null)
                return NotFound(new ApiResponse(404, "this Blog not exist!"));

            var _blog = _mapper.Map<Blog, BlogDto>(blog);

            // add all comments for this blog & count Blog's comments
            var comments = await _commentRepo.ListAsync(new GetBlogCommentsSpeci(_blog.Id));
            _blog.BlogComments = _mapper.Map<IReadOnlyList<BlogComment>, IReadOnlyList<BlogCommentDto>>(comments);
            _blog.CommentsCount = _blog.BlogComments.Count();

            //count the like and Dislike
            _blog.LikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:true));
            _blog.DislikesCount = await _likeRepo.CountAsync(new CountLikeBlogSpeci(blog.Id, like:false));

            // Get all the categories for this blog
            IReadOnlyList<BlogCategoryList> categories = await _blogCategoryListRepo.ListAsync(new GetBlogCategoriesListSpeci(_blog.Id));
            _blog.BlogCategoriesList = _mapper.Map<IReadOnlyList<BlogCategoryList>, IReadOnlyList<BlogCategoryListDto>>(categories);
            //  and get their names in the blog language
            foreach (var cat in _blog.BlogCategoriesList)
                cat.Name = (await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoryNameSpeci(cat.BlogCategoryId, blog.LanguageId))).Name;

            // get Blog's images;
            IReadOnlyList<UploadBlogImagesList> imagesList = await _uploadImageRepo.ListAsync(new getBlogImagesListOrDefaultImageSpeci(_blog.Id, defaultImg: true)) ;
            _blog.BlogImagesList = _mapper.Map<IReadOnlyList<UploadBlogImagesList>, IReadOnlyList<BlogImageDto>>(imagesList);

            
            return Ok(_blog);
        }


        [Authorize(Roles = "SuperAdmin, Admin, Editor")]
        // Post: Create a Blog
        [HttpPost("CreateBlog")]
        public async Task<ActionResult<string>> CreateBlog([FromForm] BlogCreateDto blog)
        {
            var user = await GetCurrentUserAsync(HttpContext.User);
            if (user == null) return Unauthorized(new ApiResponse(401));

            //check id category existing, example: Convert string "[1, 2, 3]" to int list
            List<int> _categoriesIds = blog.Categories.Trim('[', ']').Split(',').Select(int.Parse).ToList();
            foreach (var id in _categoriesIds)
            {
                // check if this Category existing in Category Source
                var categorySource = await _blogSourceCategoryRepo.ModelDetailsAsync(new GetBlogSourceCategories(id));
                if (categorySource == null) 
                    return NotFound(new ApiResponse(404, "Category Id:" + id + ", Not exist!"));
            }

            // mapping form BlogCreateDto -> Blog to add it in databse
            Blog newBlog = _mapper.Map<BlogCreateDto, Blog>(blog);
            newBlog.UserId = user.Id;
            newBlog.AddedDateTime = DateTime.Now;
            newBlog.UserModifiedId = user.Id;
            newBlog.ModifiedDate = DateTime.Now;

            if (await _blogRepo.AddAsync(newBlog))
            {
                if (await _blogRepo.SaveChangesAsync())
                {
                    // Add categories for this Blog
                    foreach (var id in _categoriesIds)
                    {
                        // add new Category
                        var newCat = new BlogCategoryList { BlogId = newBlog.Id, BlogCategoryId = id };
                        if (! await _blogCategoryListRepo.AddAsync(newCat))
                        {
                            // if not success create of any Blog's Categories, delete new Blog
                            await _blogRepo.RemoveAsync(newBlog);
                            await _blogRepo.SaveChangesAsync();
                            return BadRequest(new ApiResponse(400, "Something Wrong! with Creating Categories"));
                        }
                    }
                    // ************************************* add blog images
                    // the first image is Default
                    bool defaultImage = true;
                    if(blog.Files !=null)
                    {
                        if (blog.Files.Count > 0 && blog.Files.Count < 6)
                        {
                            List<string> uploadedFilesPath = new List<string>(); 
                            foreach (var file in blog.Files)
                            {
                                if (file != null && file.Length > 0)
                                {
                                    // upload the image and return the new file name
                                    var fileName = await _uploadImageRepo.UploadFileAsync(file, _webHostEnvironment.WebRootPath + BLOG_IMAGE_DIRECTORY);
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        var blogImage = new UploadBlogImagesList
                                        {
                                            Name = fileName,
                                            Path = BLOG_IMAGE_DIRECTORY + fileName,
                                            BlogId = newBlog.Id,
                                            Default = defaultImage,
                                            UserId = user.Id,
                                            UploadTypeId = 3 // Blog type
                                        };
                                        // first image is default
                                        defaultImage = false;

                                        // to delete these files from the server if not successful to add the Blog
                                        uploadedFilesPath.Add(BLOG_IMAGE_DIRECTORY + fileName);

                                        // add BlogImagesList Model
                                        if (!await _uploadImageRepo.AddAsync(blogImage))
                                        {
                                            // if not success add of any Blog's images, delete the new Blog with all uploadded images in server
                                            await _blogRepo.RemoveAsync(newBlog);
                                            await _blogRepo.SaveChangesAsync();
                                            foreach (var filePath in uploadedFilesPath)
                                                await _uploadImageRepo.DeleteFilesFromServerAsync(_webHostEnvironment.WebRootPath + filePath);

                                            return BadRequest( new ApiResponse(400, "Something Wrong! with update uploadBlogImageslist images"));
                                        }
                                    }
                                    else
                                        return BadRequest(new ApiResponse(400, "Something Wrong! with Upload images"));
                                }
                            }
                        }
                        else
                            return BadRequest(new ApiResponse(400, "The maximum number of photos is 5"));
                    }
                    // update database after add all images and Categories
                    await _blogCategoryListRepo.SaveChangesAsync();
                    return Ok("Add Blog successfully");
                }
            }
            return BadRequest(new ApiResponse(400, "Something Wrong! with add new Blog"));
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
