using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class BlogsWithCategoriesSpecification : BaseSpecification<Blog>
    {
        public BlogsWithCategoriesSpecification()
        {
            AddInclude(x => x.Language);
        }
        public BlogsWithCategoriesSpecification(int id) : base(x=>x.Id == id)
        {
            AddInclude(x => x.Language);
        }
    }
}
