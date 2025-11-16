using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantService.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }catch(Exception ex)
            {
                await HandleExceptionAsync(context , ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception is InvalidOperationException ? 400 : 500;
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new {error = exception.Message});
            return context.Response.WriteAsync(json);
        }
    }
}