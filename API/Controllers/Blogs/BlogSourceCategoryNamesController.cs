using API.ErrorsHandlers;
using Core.Dtos.Blogs;
using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Specifications.Blogs;
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
    public class BlogSourceCategoryNamesController : ControllerBase
    {
        private readonly IGenericRepository<BlogSourceCategoryName> _blogSourceCatNameRepo;
        private readonly IGenericRepository<BlogCategoryList> _blogCategoryListRepo;

        public BlogSourceCategoryNamesController(IGenericRepository<BlogSourceCategoryName> blogSourceCatNameRepo,
                                                 IGenericRepository<BlogCategoryList> blogCategoryListRepo)
        {
            _blogSourceCatNameRepo = blogSourceCatNameRepo;
            _blogCategoryListRepo = blogCategoryListRepo;
        }

        // GET: api/BlogCaregoryName/GetSourceBlogCategoryName
        [HttpGet("ReadListSourceBlogCategoryNames")]
        public async Task<IReadOnlyList<BlogSourceCategoryName>> ReadListSourceBlogCategoryNames()
        {
            return await _blogSourceCatNameRepo.ListAsync(new GetBlogSourceCategoriesOrSourceCategoryByIdSpeci());
        }

        [HttpGet("ReadSourceBlogCategoryName")]
        public async Task<BlogSourceCategoryName> ReadSourceBlogCategoryName([FromForm] int id)
        {
            return await _blogSourceCatNameRepo.ModelDetailsAsync(new GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(id));
        }

        [HttpPost("CreateSourceCategoryName")]
        public async Task<ActionResult<string>> CreateSourceCategoryName([FromForm] string name)
        {
            // check if this Source Name existing before
            var sourceName = await _blogSourceCatNameRepo.ModelDetailsAsync(new GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(name));
            if (sourceName != null) return BadRequest(new ApiResponse(400, $"This {name} category existing before!"));

            if (await _blogSourceCatNameRepo.AddAsync(new BlogSourceCategoryName { Name = name }))
                if (await _blogSourceCatNameRepo.SaveChangesAsync())
                    return Ok($"create Source Blog's Category /{name}/ Successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));
        }

        [HttpPut("UpdateSourceCategoryName")]
        public async Task<ActionResult<string>> UpdateSourceCategoryName([FromForm] BlogSourceCategoryDto sourcCate)
        {
            // check if this Source existing before
            var catSource = await _blogSourceCatNameRepo.ModelDetailsAsync(new GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(sourcCate.Id));
            if (catSource == null) return BadRequest(new ApiResponse(400, $"This category not existing!"));

            // check if this Source Name existing before
            var sourceName = await _blogSourceCatNameRepo.ModelDetailsAsync(new GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(sourcCate.Name));
            if (sourceName != null) return BadRequest(new ApiResponse(400, $"This {sourcCate.Name} category existing before!"));

            // get old name
            var oldName = catSource.Name;

            catSource.Name = sourcCate.Name;
            if (await _blogSourceCatNameRepo.UpdateAsync(catSource))
                if (await _blogSourceCatNameRepo.SaveChangesAsync())
                    return Ok($"update category: {oldName} to {sourcCate.Name} successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));

        }

        [HttpDelete("DeleteSourceCategoryName")]
        public async Task<ActionResult<string>> DeleteSourceCategoryName([FromForm] int id)
        {
            // check if this Source existing before
            var catSource = await _blogSourceCatNameRepo.ModelDetailsAsync(new GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(id));
            if (catSource == null) return BadRequest(new ApiResponse(400, $"This category not existing!"));

            // delete rows in BlogCategoryList with this Category source Name
            var blogCatList = await _blogCategoryListRepo.ListAsync(new GetAllBlogCategorisListForThisCategorySpeci(id));
            foreach (var blgCat in blogCatList)
                await _blogCategoryListRepo.RemoveAsync(blgCat);

            if (await _blogSourceCatNameRepo.RemoveAsync(catSource))
                if (await _blogSourceCatNameRepo.SaveChangesAsync())
                    return Ok($"Delete category: {catSource.Name} successfully");

            return BadRequest(new ApiResponse(400, $"somthing wrong!"));
        }
    }
}
