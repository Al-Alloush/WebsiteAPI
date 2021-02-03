using API.ErrorsHandlers;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions.ApiServices
{
    public static class OverrideApiBehaviorOptionsEx
    {

        public static IServiceCollection OverrideApiBehaviorOptions(this IServiceCollection services)
        {


            //************************************************ override the behavior of ``[ ApiController ]
            // Configer the ApiBehaviorOptions type service
            services.Configure<ApiBehaviorOptions>(options =>
            {// pass some option what we want configer
                options.InvalidModelStateResponseFactory = actionContext =>
                { /*inside the actionContext is where we can get our model state errors and that's what the API attribute is 
                    using to populate any errors that are related to validation and add them into a model state dictionar*/

                    /*extract the errors if there are any and populates the error messages into an array and 
                    that's the array will pass into our ApiValidationErrorResponse class into the errors property */
                    var errors = actionContext.ModelState       /* ModelState is a dictionary type of object. */
                        .Where(e => e.Value.Errors.Count > 0)   /* check if here any Error */
                        .SelectMany(x => x.Value.Errors)        /* select all of the errors */
                        .Select(x => x.ErrorMessage).ToArray(); /* select just the error messages */
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse); /* pass ApiValidationErrorResponse with all errors*/
                };
            });

            return services;

        }
    }
}
