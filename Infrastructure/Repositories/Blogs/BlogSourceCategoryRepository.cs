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
    public class BlogSourceCategoryRepository : BaseRepository<BlogSourceCategoryName> , IBlogSourceCategoryRepository
    {
        private readonly AppDbContext _context;

        public BlogSourceCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAllBlogCategoryList(int id)
        {
            try
            {
                var BlogCategoryList = await _context.BlogCategoryList.Where(x => x.BlogCategoryId == id)
                                                                      .ToListAsync();
                foreach (var blcat in BlogCategoryList)
                {
                    _context.Remove(blcat);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }

        public async Task<BlogSourceCategoryName> ModelAsync(string name)
        {
            BlogSourceCategoryName blogSourceCateg = await _context.BlogSourceCategoryName.Where(x => x.Name == name)
                                                                                          .FirstOrDefaultAsync();
            return blogSourceCateg;
        }
    }
}
