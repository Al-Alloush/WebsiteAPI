using Core.Interfaces.Repository;
using Core.Models.Blogs;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.SpecEvaluators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericBaseBlogRepository<T> : IGenericBaseBlogRepository<T> where T : BaseBlogModel
    {
        private readonly AppDbContext _context;

        public GenericBaseBlogRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<T> GetModelWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return BlogSpecificationsEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}
