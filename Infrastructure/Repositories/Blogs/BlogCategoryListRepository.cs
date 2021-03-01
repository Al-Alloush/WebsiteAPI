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
    public class BlogCategoryListRepository : BaseRepository<BlogCategoryList>, IBlogCategoryListRepository
    {
        private readonly AppDbContext _context;

        public BlogCategoryListRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<BlogCategoryList>> GetBlogCategoryListByBlogIdAsync(int blogId)
        {
            return await _context.BlogCategoryList.Where(x => x.BlogId == blogId).ToListAsync();
        }
    }
}
