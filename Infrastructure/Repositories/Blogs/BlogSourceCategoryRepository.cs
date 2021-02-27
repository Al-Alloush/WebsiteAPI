using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Blogs
{
    public class BlogSourceCategoryRepository : BaseRepository<BlogSourceCategoryName> , IBlogSourceCategoryRepository
    {
        private readonly AppDbContext _context;

        public BlogSourceCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<BlogSourceCategoryName> ModelAsync(int id)
        {
            BlogSourceCategoryName blogSourceCateg = await _context.BlogSourceCategoryName.FindAsync(id);
            return blogSourceCateg;
        }

        public override Task<BlogSourceCategoryName> ModelAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
