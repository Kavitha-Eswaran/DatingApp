using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _next = next;
        }

        //It is a required method in this middleware and it's going to be an async task.
        public async Task InvokeAsync(HttpContext context)
        {
            //To test this exception handling, we have created a new angular component 'test-errors' and calling our buggy API controller methods to produce some errors at runtime.
            try
            {
                //So the first thing we're going to do is get a context and to simply pass this on to the next piece of middleware. Now, this piece of middleware is going to live at the very top of our middleware, and anything below this if we have 17 bits of middleware that are invoked next at some point and, if any of them get an exception, they're going to throw the exception up and up and up until they reach something that can handle the exception. And then because our exception middleware is going to be at the top of that tree, then we're going to catch the exception inside here. So we're going to use catch exception.
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()
                //And just in case this StackTrace is null, we need to add the question mark there and then we can use ToString(). we don't want to cause an exception in a exception handling middleware. Who knows what could happen then? We might break the Internet. But just in case this is null, then we're going to prevent any exceptions from this by adding that question mark.
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                //The below is for non development environment.
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                // we'll create some options, because what we can do is send back this as a Json. now by default, we want our Json responses to go back in Camel case. So we're going to create some options to enable this, because we need to serialize this response into JSON response.
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}