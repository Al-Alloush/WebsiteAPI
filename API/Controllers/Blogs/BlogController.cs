using API.ControllerServices.Blogs;
using API.ErrorsHandlers;
using Core.Dtos.Blogs;
using Core.Helppers;
using Core.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly BlogService _blogService;

        public BlogController(UserManager<AppUser> userManager, BlogService blogService)
        {
            _userManager = userManager;
            _blogService = blogService;
        }

        [HttpGet("GetAllBlogCardList")]
        public async Task<ActionResult<Pagination<BlogCardDto>>> GetAllBlog([FromForm] SpecificParameters par)
        {
            // check if this user existing to use this object to get the user's languages, that has selected
            var currentUser = await GetCurrentUserAsync(HttpContext.User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            Pagination<BlogCardDto> pagenation = await _blogService.GetAllBlogAsync(par, currentUser);
            if (pagenation==null)
                return null;

            return pagenation;
        }

        [HttpGet("GetBlogDetails")]
        public async Task<ActionResult<BlogDto>> GetBlogDetails([FromForm] int id)
        {
            var blog = await _blogService.GetBlogDetailsAsync(id);
            return Ok(blog);
        }


        [Authorize(Roles = "SuperAdmin, Admin, Editor")]
        // Post: Create a Blog
        [HttpPost("CreateBlog")]
        public async Task<ActionResult<string>> CreateBlog([FromForm] BlogCreateDto blog)
        {
            var currentUser = await GetCurrentUserAsync(HttpContext.User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var status = await _blogService.CreateBlogAsync(blog, currentUser);
            return Ok(status);
        }

        [Authorize(Roles = "SuperAdmin, Admin, Editor")]
        // Put: update the Blog
        [HttpPut("UpdateBlog")]
        public async Task<ActionResult<string>> UpdateBlog([FromForm] BlogUpdateDto blog)
        {
            // get Current user
            var currentUser = await GetCurrentUserAsync(HttpContext.User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var status = await _blogService.UpdateBlogAsync(blog, currentUser);
            return Ok(status);

        }

        // Post: Add New Blog's Image
        [HttpPost("AddNewBlogImages")]
        [Authorize(Roles = "SuperAdmin, Admin, Editor")]
        public async Task<ActionResult<string>> AddNewBlogImages([FromForm] BlogAddImageDto blogImage)
        {

            // get Current user
            var currentUser = await GetCurrentUserAsync(HttpContext.User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var status = await _blogService.AddNewBlogImages(blogImage, currentUser);
            return Ok(status);

        }

        [HttpDelete("DeleteBlogImage")]
        [Authorize(Roles = "SuperAdmin, Admin, Editor")]
        public async Task<ActionResult<string>> DeleteBlogImage([FromForm] int imageId)
        {
            // get Current user
            var currentUser = await GetCurrentUserAsync(HttpContext.User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var status = await _blogService.DeleteBlogImageAsync(imageId, currentUser);
            return Ok(status);
        }


        // Delete: Delete the Blog
        [HttpDelete("DeleteBlog")]
        [Authorize(Roles = "SuperAdmin, Admin, Editor")]
        public async Task<ActionResult<string>> DeleteBlog([FromForm] int blogId)
        {
            // get Current user
            var currentUser = await GetCurrentUserAsync(HttpContext.User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var status = await _blogService.DeleteBlogAsync(blogId, currentUser);
            return Ok(status);


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
