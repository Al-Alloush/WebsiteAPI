using API.ErrorsHandlers;
using API.Extensions.ApiServices;
using AutoMapper;
using Core.Helppers;
using Core.Interfaces.Repository;
using Infrastructure.Data;
using Infrastructure.Data.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Add Mapping Tools
            services.AddAutoMapper(typeof(MappingProfiles));

            // Add Sqlite DB Service
            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlite(Configuration.GetConnectionString("DbConnection"));
            });

            // Add IdentityAndTokenServices from Extension file
            services.AddIdentityServices(Configuration);

            //
            services.AddSwaggerDocumentation(Configuration);

            // override the behavior of ``[ ApiController ]`` Validation Error
            services.OverrideApiBehaviorOptions();

            // to use EmailSmsSender Service in API project
            services.AddScoped<EmailSmsSenderService>();

            // add all Repositories:
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // handling exceptions just in developer mode.
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            app.UseMiddleware<ExceptionMiddleware>();

            // if request commes into API Server don't have and EndPoint match that request, this middleware redirect to ErrorController.cs 
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            // to worke Authentication JWT Service
            app.UseAuthentication();
            app.UseAuthorization();

            // API/Extensions/SwaggerServiceExtensions.cs
            app.UseSwaggerDocumention(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
