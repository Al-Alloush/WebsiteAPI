using Core.Helppers;
using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetBlogsListPaginationOrBlogDetailsSpeci : BaseSpecification<Blog>
    {

        public GetBlogsListPaginationOrBlogDetailsSpeci(SpecificParameters par, List<string> userLangs, bool emptyConstructor = false) :
            base(x => x.Publish == true &&
                   /* filter the blogs By CategoryId, one to many relationships.
                   * use or else expression to execute the right side if condition par.CategoryId.HasValue == false, !to change value from true to false */
                   (!par.CategoryId.HasValue || x.BlogCategoriesList.OrderByDescending(c => c.Id).First().BlogCategoryId == par.CategoryId) &&
                   userLangs.Contains(x.LanguageId)
                )
        {
            // if was emptyConstructor == true then we need empty constructor
            if (!emptyConstructor)
            {

                AddInclude(x => x.Language);

                // At the beginning of the sorting, the blogs are placed to remain at the top and then sorted by date of issue
                switch (par.Sort)
                {
                    case "dateAsc":
                        AddOrderByDescending(x => x.AtTop);
                        AddThenOrderBy(x => x.ReleaseDate);
                        break;
                    default:
                        AddOrderByDescending(x => x.AtTop);
                        AddThenOrderByDescending(x => x.ReleaseDate);
                        break;
                }
                /*  
                .Skip() and .Take() It must be at the end in order for pages to be created after filtering and searching
                how many do we want to Skip:
                minus one here because we want start from 0, PageSize=5 (PageIndex=1 - 1)=0
                5x0=0 this is start page    */
                ApplyPaging(par.PageSize * (par.PageIndex - 1), par.PageSize);
            }

        }

        public GetBlogsListPaginationOrBlogDetailsSpeci(int id) : base(x => x.Id == id && x.Publish == true)
        {
            AddInclude(x => x.Language);
        }

        
    }
}
