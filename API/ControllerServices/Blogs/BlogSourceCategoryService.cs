using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ControllerServices.Blogs
{
    public class BlogSourceCategoryService
    {
        private readonly IBlogSourceCategoryRepository _blogSourceCategoryRepo;

        public BlogSourceCategoryService(IBlogSourceCategoryRepository blogSourceCategoryRepo)
        {
            _blogSourceCategoryRepo = blogSourceCategoryRepo;
        }

        public async Task<IReadOnlyList<BlogSourceCategoryName>> ReadListSourceBlogCategoryNamesAsync()
        {
            var sourceCategs = await _blogSourceCategoryRepo.ListAsync();
            return sourceCategs;
        }

        public async Task<BlogSourceCategoryName> ReadSourceBlogCategoryNameAsync(int id)
        {
            var sourceCateg = await _blogSourceCategoryRepo.ModelAsync(id);
            return sourceCateg;
        }

        public async Task<BlogSourceCategoryName> ReadSourceBlogCategoryNameAsync(string name)
        {
            var sourceCateg = await _blogSourceCategoryRepo.ModelAsync(name);
            return sourceCateg;
        }

        public async Task<bool> CreateSourceCategoryNameAsync(string name)
        {
            var sourceCateg = new BlogSourceCategoryName
            {
                Name = name
            };
            if (await _blogSourceCategoryRepo.AddAsync(sourceCateg))
                if (await _blogSourceCategoryRepo.SaveChangesAsync())
                    return true;
            return false;
        }

        public async Task<bool> UpdateSourceCategoryNameAsync(BlogSourceCategoryName sourceCateg, string name)
        {
            sourceCateg.Name = name;

            if (await _blogSourceCategoryRepo.UpdateAsync(sourceCateg))
                if (await _blogSourceCategoryRepo.SaveChangesAsync())
                    return true;
            return false;
        }
        public async Task<bool> DeleteSourceCategoryNameAsync(BlogSourceCategoryName sourceCateg)
        {
            // delete rows in BlogCategoryList with this Category source Name
            if (await _blogSourceCategoryRepo.DeleteAllBlogCategoryList(sourceCateg.Id))
                if (await _blogSourceCategoryRepo.RemoveAsync(sourceCateg))
                    if (await _blogSourceCategoryRepo.SaveChangesAsync())
                        return true;
            return false;
        }
    }
}
