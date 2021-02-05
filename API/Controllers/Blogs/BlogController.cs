using API.ErrorsHandlers;
using AutoMapper;
using Core.Dtos.Blogs;
using Core.Helppers;
using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Models.Identity;
using Core.Specifications.Blogs;
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
    [Authorize(Roles = "SuperAdmin, Admin, Editor")]
    public class BlogController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericBaseBlogRepository<Blog> _blogRepo;
        private readonly IMapper _mapper;

        public BlogController(UserManager<AppUser> userManager, 
                                IGenericBaseBlogRepository<Blog> blogRepo,
                                IMapper mapper
                                )
        {
            _userManager = userManager;
            _blogRepo = blogRepo;
            _mapper = mapper;
        }

        [HttpGet("GetAllBlogCardList")]
        public async Task<ActionResult<Pagination<BlogCardDto>>> GetAllBlog([FromForm] SpecificParameters par)
        {
            AppUser user = await GetCurrentUserAsync(HttpContext.User);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var spec = new BlogsWithCategoriesSpecification(par);


            IReadOnlyList<Blog> blogs = await _blogRepo.ListAsync(spec);
            IReadOnlyList<BlogCardDto> _blogs = _mapper.Map<IReadOnlyList<Blog>, IReadOnlyList< BlogCardDto>> (blogs);

            return Ok(_blogs);
        }

        [HttpGet("GetBlogDetails")]
        public async Task<ActionResult<BlogDto>> GetBlogDetails([FromForm] int id)
        {
            AppUser user = await GetCurrentUserAsync(HttpContext.User);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var spec = new BlogsWithCategoriesSpecification(id);

            Blog blog = await _blogRepo.GetModelWithSpecAsync(spec);
            BlogDto _blog = _mapper.Map<Blog, BlogDto>(blog);

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
