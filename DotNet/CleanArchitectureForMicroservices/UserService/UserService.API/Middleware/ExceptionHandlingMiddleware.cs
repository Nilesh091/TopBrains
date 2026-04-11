using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserService.API.DTOs;

namespace UserService.API.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An unhandled exception occurred");
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";

      var response = new ApiResponse<object>();

      switch (exception)
      {
        case ArgumentNullException argNullEx:
          context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
          response = ApiResponse<object>.FailResponse(
              "Invalid input parameter",
              new List<string> { argNullEx.Message });
          break;

        case InvalidOperationException invalidOpEx:
          context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
          response = ApiResponse<object>.FailResponse(
              "Invalid operation",
              new List<string> { invalidOpEx.Message });
          break;

        case UnauthorizedAccessException unauthorizedEx:
          context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
          response = ApiResponse<object>.FailResponse(
              "Unauthorized",
              new List<string> { unauthorizedEx.Message });
          break;

        case KeyNotFoundException keyNotFoundEx:
          context.Response.StatusCode = (int)HttpStatusCode.NotFound;
          response = ApiResponse<object>.FailResponse(
              "Resource not found",
              new List<string> { keyNotFoundEx.Message });
          break;

        default:
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          response = ApiResponse<object>.FailResponse(
              "An unexpected error occurred",
              new List<string> { "Internal Server Error" });
          break;
      }

      return context.Response.WriteAsJsonAsync(response, new JsonSerializerOptions
      {
        PropertyNamingPolicy = null
      });
    }
  }
}
