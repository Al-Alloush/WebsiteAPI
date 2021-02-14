using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogCategoriesOrBlogCategoryByIdSpeci : BaseSpecification<BlogCategory>
    {
        public GetBlogCategoriesOrBlogCategoryByIdSpeci()
        {
            AddInclude(x => x.Language);
            AddOrderByDescending(x => x.LanguageId);
            AddThenOrderBy(x => x.SourceCategoryId);
            
        }

        public GetBlogCategoriesOrBlogCategoryByIdSpeci(string langId) 
            : base(x => x.LanguageId == langId)
        {
            AddInclude(x => x.Language);
            AddOrderByDescending(x => x.SourceCategoryId);
            AddThenOrderBy(x => x.Id);
        }

        public GetBlogCategoriesOrBlogCategoryByIdSpeci(int catId) 
            : base(x => x.Id == catId)
        {
            AddInclude(x => x.Language);
        }

        public GetBlogCategoriesOrBlogCategoryByIdSpeci(int sourceCatId, string lang) 
            : base(x => x.SourceCategoryId == sourceCatId && x.LanguageId == lang)
        {
            AddInclude(x => x.Language);
        }

        public GetBlogCategoriesOrBlogCategoryByIdSpeci(int sourceCatId, string lang, string name) 
            : base(x => x.SourceCategoryId == sourceCatId && x.LanguageId == lang && x.Name == name)
        {
        }

    }
}
