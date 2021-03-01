using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository.Blogs
{
    public interface IBlogCategoryListRepository : IBaseRepository<BlogCategoryList>
    {
        Task<IReadOnlyList<BlogCategoryList>> GetBlogCategoryListByBlogIdAsync(int blogId);
    }
}
