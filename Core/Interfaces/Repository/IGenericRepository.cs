using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> ModelDetailsAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

        Task<bool> AddAsync(T model);

        Task<bool> SaveChangesAsync();

        Task<bool> RemoveAsync(T model);
    }
}
