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

        public override Task<BlogCategory> ModelAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogCategory> ModelAsync(int sourceCatId, string langId, string name)
        {
            var category = await _context.BlogCategory.Include(b => b.Language)
                                                     .Where(c => c.SourceCategoryId == sourceCatId && c.LanguageId == langId && c.Name == name)
                                                     .FirstOrDefaultAsync();
            return category;
        }

        

        //public async Task<bool> AddAsync(BlogCategory category)
        //{
        //    try
        //    {
        //        var result = await _context.AddAsync(category);
        //        if (result.State.ToString() == "Added")
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }


        //}

        //public async Task<bool> UpdateAsync(BlogCategory category)
        //{
        //    try
        //    {
        //        _context.Set<BlogCategory>().Attach(category);
        //        _context.Entry(category).State = EntityState.Modified;
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> RemoveAsync(BlogCategory category)
        //{
        //    try
        //    {
        //        EntityEntry<BlogCategory> result = _context.Remove(category);
        //        if (result.State.ToString() == "Deleted")
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> SaveChangesAsync()
        //{
        //    try
        //    {
        //        // if _context.SaveChangesAsync() bigger than 0 (if success equel 1)  return true (Success Save changes)
        //        var result = await _context.SaveChangesAsync();
        //        if (result > 0)
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
