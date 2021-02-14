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
        private readonly IGenericRepository<BlogCategory> _blogCategoryRepo;
        private readonly IMapper _mapper;

        public BlogCategoryController(IGenericRepository<BlogCategory> blogCategoryRepo,
                                      IMapper mapper)
        {
            _blogCategoryRepo = blogCategoryRepo;
            _mapper = mapper;
        }

        // GET: api/BlogCaregory/
        [HttpGet("ReadBlogCategories")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories()
        {
            var cats =  await _blogCategoryRepo.ListAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci());
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("ReadBlogCategoriesBylangId")]
        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategories(string langId)
        {
            var cats = await _blogCategoryRepo.ListAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(langId));
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }

        [HttpGet("ReadBlogCategory")]
        public async Task<BlogCategoryDto> ReadBlogCategories( int id, string langId)
        {
            var cat = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(id, langId));
            var  _cat = _mapper.Map<BlogCategory, BlogCategoryDto>(cat);
            return _cat;
        }

        [HttpPost("CreateBlogCategory")]
        public async Task<ActionResult<string>> CreateBlogCategory([FromForm] int sourceCateId, [FromForm] string langId, [FromForm] string name)
        {
            // check if this Source Name existing before
            var sourceName = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId, name));
            if (sourceName != null) return BadRequest(new ApiResponse(400, $"This {name} category existing before!"));

            if (await _blogCategoryRepo.AddAsync(new BlogCategory {SourceCategoryId = sourceCateId, LanguageId=langId, Name = name}))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return Ok($"create Blog's Category /{name}/ Successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));
        }

        [HttpPut("UpdateCategoryName")]
        public async Task<ActionResult<string>> UpdateCategoryName([FromForm] int sourceCateId, [FromForm] string langId, [FromForm] string newName)
        {
            // check if this Source existing before
            var catSource = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId));
            if (catSource == null) return BadRequest(new ApiResponse(400, $"This category not existing!"));

            // get old name
            var oldName = catSource.Name;

            catSource.Name = newName;
            if (await _blogCategoryRepo.UpdateAsync(catSource))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return Ok($"update category: {oldName} to {newName} successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));
        }

        [HttpDelete("DeleteBlogCategory")]
        public async Task<ActionResult<string>> DeleteSourceCategoryName([FromForm] int sourceCateId, [FromForm] string langId)
        {
            // check if this Source existing before
            var catSource = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId));
            if (catSource == null) return BadRequest(new ApiResponse(400, $"This category not existing!"));

            if (await _blogCategoryRepo.RemoveAsync(catSource))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return Ok($"Delete category: {catSource.Name} successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));
        }
    }
}
