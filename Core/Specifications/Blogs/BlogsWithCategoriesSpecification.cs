using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class BlogsWithCategoriesSpecification : BaseSpecification<Blog>
    {
        public BlogsWithCategoriesSpecification(string sort) : base(x=>x.Publish == true)
        {
            AddInclude(x => x.Language);

            // At the beginning of the sorting, the blogs are placed to remain at the top and then sorted by date of issue
            switch (sort)
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
        }
        public BlogsWithCategoriesSpecification(int id) : base(x=>x.Id == id)
        {
            AddInclude(x => x.Language);
        }
    }
}
