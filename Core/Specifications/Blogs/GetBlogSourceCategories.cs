using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogSourceCategories : BaseSpecification<BlogSourceCategoryName>
    {
        public GetBlogSourceCategories(int categoryId) : base(c => c.Id == categoryId)
        {

        }
    }
}
