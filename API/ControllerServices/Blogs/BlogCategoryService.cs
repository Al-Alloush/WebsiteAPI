using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.ControllerServices.Blogs
{
    public class BlogCategoryService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepo;


        public BlogCategoryService(IBlogCategoryRepository blogCategoryRepo)
        {
            _blogCategoryRepo = blogCategoryRepo;
        }

        public async Task<IReadOnlyList<BlogCategory>> ReadBlogCategoriesAsync()
        {
            var cats = await _blogCategoryRepo.ListAsync();
            return cats;
        }

        public async Task<IReadOnlyList<BlogCategory>> ReadBlogCategoriesAsync(int sourceCatId)
        {
            var cats = await _blogCategoryRepo.ListAsync(sourceCatId);

            return cats;
        }

        public async Task<IReadOnlyList<BlogCategory>> ReadBlogCategoriesAsync(string langId)
        {
            var cats = await _blogCategoryRepo.ListAsync(langId);

            return cats;
        }

        public async Task<IReadOnlyList<BlogCategory>> ReadBlogCategoriesAsync(int sourceCatId, string langId)
        {
            var cats = await _blogCategoryRepo.ListAsync(sourceCatId, langId);

            return cats;
        }

        public async Task<BlogCategory> ReadBlogCategoryByIdAsync(int id)
        {
            var cat = await _blogCategoryRepo.ModelAsync(id);

            return cat;
        }

        public async Task<BlogCategory> ReadBlogCategoriesAsync(int sourceCatId, string langId, string name)
        {
            var cat = await _blogCategoryRepo.ModelAsync(sourceCatId, langId, name);
            return cat;
        }


        public async Task<bool> CreateBlogCategoryAsync(int sourceCateId, string langId, string name)
        {
            // create new BlogCategory Model
            var newCategory = new BlogCategory
            {
                SourceCategoryId = sourceCateId,
                LanguageId = langId,
                Name = name
            };

            if (await _blogCategoryRepo.AddAsync(newCategory))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return true;

            return false;
        }

        public async Task<bool> UpdateBlogCategoryAsync(BlogCategory category, int newSourceCateId, string newLangId, string newName)
        {
            category.Name = newName;
            category.SourceCategoryId = newSourceCateId;
            category.LanguageId = newLangId;

            if (await _blogCategoryRepo.UpdateAsync(category))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return true;

            return false;
        }

        public async Task<bool> DeleteCategoryNameAsync(BlogCategory category)
        {

            if (await _blogCategoryRepo.RemoveAsync(category))
                if (await _blogCategoryRepo.SaveChangesAsync())
                    return true;

            return false;
        }
    }
}
