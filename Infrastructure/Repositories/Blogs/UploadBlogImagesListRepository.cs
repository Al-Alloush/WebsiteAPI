using Core.Interfaces.Repository.Blogs;
using Core.Models.Uploads;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Blogs
{
    public class UploadBlogImagesListRepository : BaseRepository<UploadBlogImagesList>, IUploadBlogImagesListRepository
    {
        private readonly AppDbContext _context;

        public UploadBlogImagesListRepository(AppDbContext context) : base (context)
        {
            _context = context;
        }
    }
}
