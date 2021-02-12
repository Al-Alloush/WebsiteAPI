using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.Blogs
{
    public class BlogAddImageDto
    {
        public List<IFormFile> Files { get; set; }
        public int BlogId { get; set; }
    }
}
