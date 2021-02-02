using AutoMapper;
using Core.Helppers;
using Core.Models.Identity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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

            /********************************************************************/
            services.AddIdentityCore<AppUser>() /*Add service for type 'Microsoft.AspNetCore.Identity.UserManager:*/
                    .AddRoles<IdentityRole>() /*Add IdentityRole service in Application:*/
                    .AddEntityFrameworkStores<AppDbContext>() /*to avoid error :Unable to resolve service for type 'Microsoft.AspNetCore.Identity.IUserStore`1 */ ; 
            /********************************************************************/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
