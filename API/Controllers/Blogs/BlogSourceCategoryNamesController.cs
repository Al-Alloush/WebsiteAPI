using API.ControllerServices.Blogs;
using API.ErrorsHandlers;
using Core.Dtos.Blogs;
using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Specifications.Blogs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Blogs
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogSourceCategoryNamesController : ControllerBase
    {
        private readonly BlogSourceCategoryService _blogSourceCategService;
        private readonly IGenericRepository<BlogSourceCategoryName> _blogSourceCatNameRepo;
        private readonly IGenericRepository<BlogCategoryList> _blogCategoryListRepo;

        public BlogSourceCategoryNamesController(BlogSourceCategoryService blogSourceCategService,
                                                IGenericRepository<BlogSourceCategoryName> blogSourceCatNameRepo,
                                                 IGenericRepository<BlogCategoryList> blogCategoryListRepo)
        {
            _blogSourceCategService = blogSourceCategService;
            _blogSourceCatNameRepo = blogSourceCatNameRepo;
            _blogCategoryListRepo = blogCategoryListRepo;
        }

        // GET: api/BlogCaregoryName/GetSourceBlogCategoryName
        [HttpGet("ReadListSourceBlogCategoryNames")]
        public async Task<IReadOnlyList<BlogSourceCategoryName>> ReadListSourceBlogCategoryNames()
        {
            return await _blogSourceCategService.ReadListSourceBlogCategoryNamesAsync();
        }

        [HttpGet("ReadSourceBlogCategoryName")]
        public async Task<BlogSourceCategoryName> ReadSourceBlogCategoryName([FromForm] int id)
        {
            return await _blogSourceCategService.ReadSourceBlogCategoryNameAsync(id);
        }

        [HttpPost("CreateSourceCategoryName")]
        public async Task<ActionResult<string>> CreateSourceCategoryName([FromForm] string name)
        {
            // check if this Source Name existing before
            var sourceName = await _blogSourceCategService.ReadSourceBlogCategoryNameAsync(name);
            if (sourceName != null) return BadRequest(new ApiResponse(400, $"This {name} category existing before!"));

            if (await _blogSourceCategService.CreateSourceCategoryNameAsync(name))
                return Ok($"create Source Blog's Category /{name}/ Successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!, with create SourcrCategory"));
        }

        [HttpPut("UpdateSourceCategoryName")]
        public async Task<ActionResult<string>> UpdateSourceCategoryName([FromForm] BlogSourceCategoryDto sourcCate)
        {
            // check if this Source existing before
            var sourceCateg = await _blogSourceCategService.ReadSourceBlogCategoryNameAsync(sourcCate.Id);
            if (sourceCateg == null) return BadRequest(new ApiResponse(400, $"This category not existing!"));
            // check if this Source Name existing before
            var sourceName = await _blogSourceCategService.ReadSourceBlogCategoryNameAsync(sourcCate.Name);
            if (sourceName != null) return BadRequest(new ApiResponse(400, $"This {sourcCate.Name} category existing before!"));
            // get old name
            var oldName = sourceCateg.Name;

            if (await _blogSourceCategService.UpdateSourceCategoryNameAsync(sourceCateg, sourcCate.Name))
                    return Ok($"update category: {oldName} to {sourcCate.Name} successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));

        }

        [HttpDelete("DeleteSourceCategoryName")]
        public async Task<ActionResult<string>> DeleteSourceCategoryName([FromForm] int id)
        {
            // check if this Source existing before
            // check if this Source existing before
            var sourceCateg = await _blogSourceCategService.ReadSourceBlogCategoryNameAsync(id);
            if (sourceCateg == null) return BadRequest(new ApiResponse(400, $"This category not existing!"));

            if (await _blogSourceCategService.DeleteSourceCategoryNameAsync(sourceCateg))
                    return Ok($"Delete category: {sourceCateg.Name} successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));
        }
    }
}
