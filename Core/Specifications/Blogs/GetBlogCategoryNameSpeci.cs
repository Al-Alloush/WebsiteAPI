using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogCategoryNameSpeci : BaseSpecification<BlogCategory>
    {
        public GetBlogCategoryNameSpeci(int catId, string catLang) : base (x=>x.SourceCategoryId == catId && x.LanguageId == catLang)
        {

        }
    }
}
