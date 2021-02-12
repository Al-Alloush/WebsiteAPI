using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogCategoriesListSpeci : BaseSpecification<BlogCategoryList>
    {
        public GetBlogCategoriesListSpeci(int blogId) : base (x=> x.BlogId == blogId)
        {

        }
    }
}
