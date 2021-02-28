using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository.Blogs
{
    public interface IBlogCommentRepository : IBaseRepository<BlogComment>
    {

        Task<IReadOnlyList<BlogComment>> ListAsync(int id);

        Task<int> CountByIdAsync(int id);
    }
}
