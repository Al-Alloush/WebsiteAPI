using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ControllerServices.Blogs
{
    public class BlogCommentService
    {
        private readonly IBlogCommentRepository _blogCommentRepo;

        public BlogCommentService(IBlogCommentRepository blogCommentRepo)
        {
            _blogCommentRepo = blogCommentRepo;
        }

        public async Task<IReadOnlyList<BlogComment>> GetCommentsByBlogIdAsync(int blogId)
        {
            var comments = await _blogCommentRepo.GetCommentsListByBlogId(blogId);
            return comments;
        }

    }
}
