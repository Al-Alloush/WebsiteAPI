using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ErrorsHandlers
{
    public class ApiResponse
    {
        // every error is going to have these two properities
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            // fancy switch expressions
            return statusCode switch
            {
                202 => "return null",
                201 => "you have made a success request",
                400 => "you have made a bad request!",
                401 => "you are not Authorized",
                403 => "you don't have the permissions",
                404 => "Resource not found!",
                405 => "Method Not Allowed, A request was made of a resource using a request method not supported by that resource; for example, using GET on a form which requires data to be presented via POST, or using PUT on a read-only resource.",
                500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to a career change",
                _ => null/* _ => the default in switch */
            };
        }
    }
}
