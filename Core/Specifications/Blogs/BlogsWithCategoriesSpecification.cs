using Core.Helppers;
using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class BlogsWithCategoriesSpecification : BaseSpecification<Blog>
    {
        public BlogsWithCategoriesSpecification(SpecificParameters par) : base(x=> x.Publish == true)
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
            //ApplyPaging(par.PageSize * (par.PageIndex - 1), par.PageSize);
        }
        public BlogsWithCategoriesSpecification(int id) : base(x=>x.Id == id)
        {
            AddInclude(x => x.Language);
        }
    }
}
