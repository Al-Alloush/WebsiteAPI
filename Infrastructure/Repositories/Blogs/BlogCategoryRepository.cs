using Core.Interfaces.Repository;
using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Blogs
{
    public class BlogCategoryRepository : IBlogCategoryRepository
    {

        private readonly AppDbContext _context;

        public BlogCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IReadOnlyList<BlogCategory>> ListAsync()
        {
            var categories = await _context.BlogCategory.Include(b => b.Language)
                                                        .OrderBy(c => c.LanguageId).ThenBy(c => c.SourceCategoryId)
                                                        .ToListAsync();
            return categories;
        }

        public async Task<IReadOnlyList<BlogCategory>> ListByLanguageIdAsync(string langId)
        {
            var categories = await _context.BlogCategory.Include(b => b.Language)
                                                        .Where(c => c.LanguageId == langId)
                                                        .OrderBy(c => c.SourceCategoryId)
                                                        .ToListAsync();
            return categories;
        }

        public async Task<BlogCategory> ModelBySourceCatIdAndLangIdAsync(int sourceCategoryNameId, string langId)
        {
            var category = await _context.BlogCategory.Include(b => b.Language)
                                                      .Where(c => c.SourceCategoryId == sourceCategoryNameId && c.LanguageId == langId)
                                                      .FirstOrDefaultAsync();
            return category;
        }

        public async Task<BlogCategory> ModelBySourceCatIdAndLangIdAsync(int sourceCategoryNameId, string langId, string name)
        {
            var category = await _context.BlogCategory.Include(b => b.Language)
                                                     .Where(c => c.SourceCategoryId == sourceCategoryNameId && c.LanguageId == langId && c.Name == name)
                                                     .FirstOrDefaultAsync();
            return category;
        }

        public async Task<bool> AddAsync(BlogCategory category)
        {
            var result = await _context.AddAsync(category);
            if (result.State.ToString() == "Added")
                return true;
            else
                return false;
        }

        public async Task<bool> SaveChangesAsync()
        {
            // if _context.SaveChangesAsync() bigger than 0 (if success equel 1)  return true (Success Save changes)
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
