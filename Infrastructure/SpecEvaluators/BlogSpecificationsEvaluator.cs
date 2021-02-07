using Core.Models.Blogs;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.SpecEvaluators
{
    public class BlogSpecificationsEvaluator<T> where T : BaseBlogModel
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            // to check if Order Blogs by Asc
            if (spec.ThenOrderBy != null)
                query = query.OrderByDescending(spec.OrderByDescending).ThenBy(spec.ThenOrderBy);

            //  if not Asc then check if Order Blogs by Desc
            if (spec.ThenOrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending).ThenByDescending(spec.ThenOrderByDescending);

            // to add Pagination in query
            if (spec.IsPagingEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            
            return query;
        }
    }
}
