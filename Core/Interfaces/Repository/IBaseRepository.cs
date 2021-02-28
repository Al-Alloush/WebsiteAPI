using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : BaseModels
    {
        Task<IReadOnlyList<T>> ListAsync();

        Task<IReadOnlyList<T>> ListAsync(int id);

        Task<T> ModelAsync(int id);

        Task<int> CountAsync(int id);

        Task<bool> AddAsync(T model);

        Task<bool> UpdateAsync(T model);

        Task<bool> RemoveAsync(T model);

        Task<bool> SaveChangesAsync();
    }
}
