using Core.Interfaces.Repository;
using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Blogs
{
    public class BlogCategoryRepository : BaseRepository<BlogCategory> , IBlogCategoryRepository
    {

        private readonly AppDbContext _context;

        public BlogCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IReadOnlyList<BlogCategory>> ListAsync()
        {
            var categories = await _context.BlogCategory.Include(b => b.Language)
                                                        .OrderBy(c => c.LanguageId).ThenBy(c => c.SourceCategoryId)
                                                        .ToListAsync();
            return categories;
        }

        public async Task<IReadOnlyList<BlogCategory>> ListAsync(int sourceCatId)
        {
            var categories = await _context.BlogCategory.Include(b => b.Language)
                                                        .Where(c => c.SourceCategoryId == sourceCatId)
                                                        .OrderBy(c => c.SourceCategoryId)
                                                        .ToListAsync();
            return categories;
        }

        public async Task<IReadOnlyList<BlogCategory>> ListAsync(string langId)
        {
            var categories = await _context.BlogCategory.Include(b => b.Language)
                                                        .Where(c => c.LanguageId == langId)
                                                        .OrderBy(c => c.SourceCategoryId)
                                                        .ToListAsync();
            return categories;
        }

        public async Task<IReadOnlyList<BlogCategory>> ListAsync(int sourceCatId, string langId)
        {
            var categories = await _context.BlogCategory.Include(b => b.Language)
                                                        .Where(c => c.SourceCategoryId == sourceCatId && c.LanguageId == langId)
                                                        .OrderBy(c => c.SourceCategoryId)
                                                        .ToListAsync();
            return categories;
        }

        public override async Task<BlogCategory> ModelAsync(int id)
        {
            var category = await _context.BlogCategory.Include(b => b.Language)
                                                      .Where(c => c.Id == id)
                                                      .FirstOrDefaultAsync();
            return category;
        }

        public async Task<BlogCategory> ModelAsync(int sourceCatId, string langId, string name)
        {
            var category = await _context.BlogCategory.Include(b => b.Language)
                                                     .Where(c => c.SourceCategoryId == sourceCatId && c.LanguageId == langId && c.Name == name)
                                                     .FirstOrDefaultAsync();
            return category;
        }

    }
}
