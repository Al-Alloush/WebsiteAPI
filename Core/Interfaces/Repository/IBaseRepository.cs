using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> ListAsync();

        Task<T> ModelAsync(int value);

        Task<T> ModelAsync(string value);

        Task<bool> AddAsync(T model);

        Task<bool> UpdateAsync(T model);

        Task<bool> RemoveAsync(T model);

        Task<bool> SaveChangesAsync();
    }
}
