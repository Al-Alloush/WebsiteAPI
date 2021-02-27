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
        [HttpGet("AllBlogCategories")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories()
        {
            var cats =  await _blogCatService.ReadBlogCategoriesAsync();
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("BlogCategoriesBy_sourceCatId")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories(int sourceCatId)
        {
            var cats = await _blogCatService.ReadBlogCategoriesAsync(sourceCatId);
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("BlogCategoriesBy_langId")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories(string langId)
        {
            var cats = await _blogCatService.ReadBlogCategoriesAsync(langId);
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("BlogCategoriesBy_langId_SourceCatId")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories(int sourceCatId, string langId)
        {
            var cats = await _blogCatService.ReadBlogCategoriesAsync(sourceCatId, langId);
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("BlogCategory")]
        public async Task<BlogCategoryDto> ReadBlogCategoriesById(int id)
        {
            var cat = await _blogCatService.ReadBlogCategoryByIdAsync(id);
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

        [HttpPut("UpdateCategoryName")]
        public async Task<ActionResult<string>> UpdateCategoryName([FromForm] int id, [FromForm] int newSourceCateId, [FromForm] string newLangId, [FromForm] string newName)
        {

            // check if this Blogcategory exist or not
            var category = await _blogCatService.ReadBlogCategoryByIdAsync(id);
            if (category == null) return BadRequest(new ApiResponse(400, "this Category not exsist"));

            var status = await _blogCatService.UpdateBlogCategoryAsync(category, newSourceCateId, newLangId, newName);
            if (status)
                return $"Update Blog's Category Successfully";

            throw new Exception($"something wrong!, with Updating a BlogsCategory");
        }

        [HttpDelete("DeleteBlogCategory")]
        public async Task<ActionResult<string>> DeleteSourceCategoryName( int id)
        {
            // check if this Blogcategory exist or not
            var category = await _blogCatService.ReadBlogCategoryByIdAsync(id);
            if (category == null) return BadRequest(new ApiResponse(400, "this Category not exsist"));

            var status = await _blogCatService.DeleteCategoryNameAsync(category);
            if (status)
                return $"Delete Blog's Category Successfully";

            throw new Exception($"something wrong!, with deleting a BlogsCategory");
        }
    }
}
