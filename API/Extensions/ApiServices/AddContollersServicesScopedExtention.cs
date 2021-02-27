using API.ControllerServices.Blogs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions.ApiServices
{
    public static class AddContollersServicesScopedExtention
    {
        public static IServiceCollection AddContollersServicesScoped(this IServiceCollection services)
        {
            services.AddScoped<BlogService>();
            services.AddScoped<BlogCategoryService>();
            services.AddScoped<BlogSourceCategoryService>();


            return services;
        }
    }
}
