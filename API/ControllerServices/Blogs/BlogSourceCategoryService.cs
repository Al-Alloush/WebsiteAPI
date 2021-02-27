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
    }
}
