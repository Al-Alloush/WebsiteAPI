using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class CountLikeBlogSpeci : BaseSpecification<BlogLike>
    {
        public CountLikeBlogSpeci(int blogId, bool like) 
            : base(x=>x.BlogId == blogId && (like || x.Like == true) && (!like || x.Dislike) )
        {

        }
    }
}
