using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IGenericBaseBlogRepository<T> where T : BaseBlogModel
    {
        Task<IReadOnlyList<T>> GetListAllAsync();
        Task<T> GetByIdAsync(int id);
    }
}
