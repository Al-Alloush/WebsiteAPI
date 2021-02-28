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
    public class BlogLikeRepository : BaseRepository<BlogLike> , IBlogLikeRepository
    {
        private readonly AppDbContext _context;

        public BlogLikeRepository(AppDbContext context) :base(context)
        {
            _context = context;
        }

        public async Task<int> CountAsync(int id, bool status)
        {
            var counts = 0;

            if (status)
                counts = await _context.BlogLike.Where(x=>x.BlogId == id && x.Like == true).CountAsync();
            else
                counts = await _context.BlogLike.Where(x => x.BlogId == id && x.Dislike == true).CountAsync();
            
            return counts;
        }
    }
}
