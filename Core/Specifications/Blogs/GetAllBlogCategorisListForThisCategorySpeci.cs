using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetAllBlogCategorisListForThisCategorySpeci : BaseSpecification<BlogCategoryList>
    {
        public GetAllBlogCategorisListForThisCategorySpeci(int id) : base (x=>x.BlogCategoryId == id)
        {

        }
    }
}
