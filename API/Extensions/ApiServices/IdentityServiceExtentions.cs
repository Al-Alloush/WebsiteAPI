using Core.Interfaces.Services;
using Core.Models.Identity;
using Infrastructure.Data;
using Infrastructure.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Extensions.ApiServices
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);

            //
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    /* if we forget to add this, we might as well just leave anonymous authentication on and a user can 
                    send up any old token they want because we would never validate that the signing key is correct.*/
                    ValidateIssuerSigningKey = true,
                    /* tell it about our issue assigning key, we need to do the same encoding we did in TokenService.cs Constractor */
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    /* same as our token issuer that we're going to add to our configurations in TokenService.cs Constractor */
                    ValidIssuer = config["Token:Issuer"],
                    /* if not just accepts any issuer of any token so we'll set that to true as well.*/
                    ValidateIssuer = true,
                    /* */
                    ValidateAudience = false
                };
            });

            services.AddScoped<ITokenService, TokenService>();
            /******************************************************************/
            // entity framework implementation of identity information stores. where we were adding the UserManager to
            // DefaultUsersAsync method in Infrastructure/Identity folder that kind of service is contained inside entity framework stores.
            services.AddIdentityCore<AppUser>() /*Add service for type 'Microsoft.AspNetCore.Identity.UserManager:*/
                    .AddRoles<IdentityRole>() /*Add IdentityRole service in Application:*/
                    .AddEntityFrameworkStores<AppDbContext>() /*to avoid error :Unable to resolve service for type 'Microsoft.AspNetCore.Identity.IUserStore`1 */
                    .AddDefaultTokenProviders() /* to use Microsoft.AspNetCore.Identity Token provider to use function like:UserManager.GenerateEmailConfirmationTokenAsync()*/
                    .AddSignInManager<SignInManager<AppUser>>() /* to inject SignInManager service need to inject another service*/;
            /********************************************************************/

            // add ``app.UseAuthentication()``; in Configure method inside ``startup.cs``, directly before ``app.UseAuthorization();`` to work Authorization service

            return services;
        }
    }
}
