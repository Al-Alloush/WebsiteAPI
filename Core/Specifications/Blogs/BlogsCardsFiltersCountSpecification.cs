using Core.Helppers;
using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class BlogsCardsFiltersCountSpecification : BaseSpecification<Blog>
    {
        public BlogsCardsFiltersCountSpecification(SpecificParameters par, List<string> userLangs) 
            : base(x => x.Publish == true && 
                /* use or else expression to execuse the right side if condetion par.CategoryId.HasValue == false, !to change value from true to false */
                (!par.CategoryId.HasValue || x.BlogCategoriesList.OrderByDescending(c => c.Id).First().BlogCategoryId == par.CategoryId) &&
                userLangs.Contains(x.LanguageId)
            )
        {


        }
    }
}
