using System;
using System.Net;
using System.Text.Json;
using GlobalExceptionHandeling.Exceptions;

namespace GlobalExceptionHandeling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";


            // ✅ FIX: Proper Status Code Handling
            var statusCode = exception switch
            {
                EmployeeNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                ExceptionType = exception.GetType().Name
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);

        }

    }
}
