﻿using Core.Interfaces.Repository.Blogs;
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
    public class BlogCommentRepository : BaseRepository<BlogComment> , IBlogCommentRepository
    {
        private readonly AppDbContext _context;

        public BlogCommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CountByIdAsync(int id)
        {
            var count = await _context.BlogComment.Where(x=>x.BlogId == id)
                                            .CountAsync();
            return count;
        }

        public async Task<IReadOnlyList<BlogComment>> ListAsync(int id)
        {
            var comments = await _context.BlogComment.Where(x => x.Id == id)
                                                     .ToListAsync();
            return comments;
        }

        public override Task<BlogComment> ModelAsync(int value)
        {
            throw new NotImplementedException();
        }
    }
}