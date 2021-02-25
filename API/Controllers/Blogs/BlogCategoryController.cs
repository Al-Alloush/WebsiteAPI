using API.ControllerServices.Blogs;
using API.ErrorsHandlers;
using AutoMapper;
using Core.Dtos.Blogs;
using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Specifications;
using Core.Specifications.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Blogs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class BlogCategoryController : ControllerBase
    {
        private readonly BlogCategoryService _blogCatService;
        private readonly IMapper _mapper;

        public BlogCategoryController(BlogCategoryService blogCatService, IMapper mapper)
        {
            _blogCatService = blogCatService;
            _mapper = mapper;
        }

        // GET: api/BlogCaregory/
        [HttpGet("ReadBlogCategories")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories()
        {
            var cats =  await _blogCatService.ReadBlogCategoriesAsync();
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("ReadBlogCategoriesBylangId")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories(string langId)
        {
            var cats = await _blogCatService.ReadBlogCategoriesAsync(langId);
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("ReadBlogCategory")]
        public async Task<BlogCategoryDto> ReadBlogCategories(int id, string langId)
        {
            var cat = await _blogCatService.ReadBlogCategoriesAsync(id, langId);
            var _cat = _mapper.Map<BlogCategory, BlogCategoryDto>(cat);
            return _cat;
        }

        [HttpPost("CreateBlogCategory")]
        public async Task<ActionResult<string>> CreateBlogCategory([FromForm] int sourceCateId, [FromForm] string langId, [FromForm] string name)
        {
            // check if this Blogcategory exist or not
            var category = await _blogCatService.ReadBlogCategoriesAsync(sourceCateId, langId, name);
            if (category != null) return BadRequest(new ApiResponse(400, "this Category exsist before"));


            var status = await _blogCatService.CreateBlogCategoryAsync(sourceCateId, langId, name);
            if (status)
                return $"create Blog's Category /{name}/ Successfully";

            throw new Exception($"something wrong!, with Add and Save changes for new BlogsCategory");
        }

        //[HttpPut("UpdateCategoryName")]
        //public async Task<ActionResult<string>> UpdateCategoryName([FromForm] int sourceCateId, [FromForm] string langId, [FromForm] string newName)
        //{
        //    var status = await _blogCatService.UpdateCategoryNameAsync(sourceCateId, langId, newName);
        //    return status;
        //}

        //[HttpDelete("DeleteBlogCategory")]
        //public async Task<ActionResult<string>> DeleteSourceCategoryName([FromForm] int sourceCateId, [FromForm] string langId)
        //{
        //    var status = await _blogCatService.DeleteSourceCategoryNameAsync(sourceCateId, langId);
        //    return status;
        //}
    }
}
