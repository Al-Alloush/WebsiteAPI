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

        public async Task<IReadOnlyList<BlogCategory>> ReadBlogCategoriesAsync(string langId)
        {
            var cats = await _blogCategoryRepo.ListByLanguageIdAsync(langId);

            return cats;
        }


        public async Task<BlogCategory> ReadBlogCategoriesAsync(int sourceCategoryNameId, string langId)
        {
            var cat = await _blogCategoryRepo.ModelBySourceCatIdAndLangIdAsync(sourceCategoryNameId, langId);

            return cat;
        }

        public async Task<BlogCategory> ReadBlogCategoriesAsync(int sourceCategoryNameId, string langId, string name)
        {
            var cat = await _blogCategoryRepo.ModelBySourceCatIdAndLangIdAsync(sourceCategoryNameId, langId, name);
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



        //public async Task<ActionResult<string>> UpdateCategoryNameAsync( int sourceCateId, string langId, string newName)
        //{
        //    // check if this Source existing before
        //    var catSource = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId));
        //    if (catSource == null) return null;

        //    // get old name
        //    var oldName = catSource.Name;

        //    catSource.Name = newName;
        //    if (await _blogCategoryRepo.UpdateAsync(catSource))
        //        if (await _blogCategoryRepo.SaveChangesAsync())
        //            return $"update category: {oldName} to {newName} successfully";

        //    throw new Exception($"Update Blog's Category, somthing wrong!");
        //}

        //public async Task<ActionResult<string>> DeleteSourceCategoryNameAsync( int sourceCateId, string langId)
        //{
        //    // check if this Source existing before
        //    var catSource = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoriesOrBlogCategoryByIdSpeci(sourceCateId, langId));
        //    if (catSource == null) return null;

        //    if (await _blogCategoryRepo.RemoveAsync(catSource))
        //        if (await _blogCategoryRepo.SaveChangesAsync())
        //            return $"Delete category: {catSource.Name} successfully";

        //    throw new Exception($"Delete Blog's Category, somthing wrong!");
        //}
    }
}
