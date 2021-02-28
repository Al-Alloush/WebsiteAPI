using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository.Blogs
{
    public interface IBlogSourceCategoryRepository : IBaseRepository<BlogSourceCategoryName>
    {

        Task<BlogSourceCategoryName> ModelAsync(string name);

        Task<bool> DeleteAllBlogCategoryList(int id);
    }
}
