using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository.Blogs
{
    public interface IBlogLikeRepository : IBaseRepository<BlogLike>
    {
        Task<int> CountAsync(int id, bool like);
    }
}
