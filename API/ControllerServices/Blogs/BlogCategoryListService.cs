using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ControllerServices.Blogs
{
    public class BlogCategoryListService
    {
        private readonly IBlogCategoryListRepository _blogCategoryListRepo;

        public BlogCategoryListService(IBlogCategoryListRepository blogCategoryListRepo)
        {
            _blogCategoryListRepo = blogCategoryListRepo;
        }

        public async Task<IReadOnlyList<BlogCategoryList>> GetBlogCategoryListByBlogIdAsync(int blogId)
        {
            return await _blogCategoryListRepo.GetBlogCategoryListByBlogIdAsync(blogId);
        }

    }
}
