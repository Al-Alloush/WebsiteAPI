using AutoMapper;
using Core.Dtos.Blogs;
using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Specifications.Blogs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.ControllerServices.Blogs
{
    public class BlogCategoryService
    {

        private readonly IGenericRepository<BlogCategory> _blogCategoryRepo;
        private readonly IMapper _mapper;

        public BlogCategoryService(IGenericRepository<BlogCategory> blogCategoryRepo,
                                      IMapper mapper)
        {
            _blogCategoryRepo = blogCategoryRepo;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategoriesAsync()
        {
            var cats = await _blogCategoryRepo.ListAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci());
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }


        public async Task<IReadOnlyList<BlogCategoryDto>> ReadBlogCategoriesAsync(string langId)
        {
            var cats = await _blogCategoryRepo.ListAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(langId));
            var _cats = _mapper.Map<IReadOnlyList<BlogCategory>, IReadOnlyList<BlogCategoryDto>>(cats);
            return _cats;
        }


        public async Task<BlogCategoryDto> ReadBlogCategoriesAsync(int id, string langId)
        {
            var cat = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(id, langId));
            var _cat = _mapper.Map<BlogCategory, BlogCategoryDto>(cat);
            return _cat;
        }


        public async Task<ActionResult<string>> CreateBlogCategoryAsync( int sourceCateId,  string langId, string name)
        {
            // check if this Source Name existing before
            var sourceName = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId, name));
            if (sourceName != null) return null;

            if (await _blogCategoryRepo.AddAsync(new BlogCategory { SourceCategoryId = sourceCateId, LanguageId = langId, Name = name }))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return $"create Blog's Category /{name}/ Successfully";

            throw new Exception ($"Create new Blog's Category, somthing wrong!");
        }


        public async Task<ActionResult<string>> UpdateCategoryNameAsync( int sourceCateId, string langId, string newName)
        {
            // check if this Source existing before
            var catSource = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId));
            if (catSource == null) return null;

            // get old name
            var oldName = catSource.Name;

            catSource.Name = newName;
            if (await _blogCategoryRepo.UpdateAsync(catSource))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return $"update category: {oldName} to {newName} successfully";

            throw new Exception($"Update Blog's Category, somthing wrong!");
        }

        public async Task<ActionResult<string>> DeleteSourceCategoryNameAsync( int sourceCateId, string langId)
        {
            // check if this Source existing before
            var catSource = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId));
            if (catSource == null) return null;

            if (await _blogCategoryRepo.RemoveAsync(catSource))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return $"Delete category: {catSource.Name} successfully";

            throw new Exception($"Delete Blog's Category, somthing wrong!");
        }
    }
}
