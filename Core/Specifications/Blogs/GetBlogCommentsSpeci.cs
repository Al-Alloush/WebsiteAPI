using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogCommentsSpeci : BaseSpecification<BlogComment>
    {
        public GetBlogCommentsSpeci(int blogId) : base (x=>x.BlogId == blogId)
        {
            AddInclude(x => x.User);
        }
    }
}
