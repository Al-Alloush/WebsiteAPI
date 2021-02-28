using Core.Interfaces.Repository;
using Core.Interfaces.Repository.Blogs;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Blogs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions.Repositories
{
    public static class RepositoriesScoped
    {

        public static IServiceCollection AddRepositoriesScoped(this IServiceCollection services)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBlogCategoryRepository), typeof(BlogCategoryRepository));
            services.AddScoped(typeof(IBlogSourceCategoryRepository), typeof(BlogSourceCategoryRepository));
            services.AddScoped(typeof(IBlogCommentRepository), typeof(BlogCommentRepository));
           

            return services;
        }
    }
}
