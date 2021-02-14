using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogSourceCategoriesOrSourceCategoryByIdSpeci : BaseSpecification<BlogSourceCategoryName>
    {
        public GetBlogSourceCategoriesOrSourceCategoryByIdSpeci()
        {

        }

        public GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(int catId) : base(x=> x.Id == catId)
        {

        }

        public GetBlogSourceCategoriesOrSourceCategoryByIdSpeci(string catName) : base(x => x.Name == catName)
        {

        }
    }
}
