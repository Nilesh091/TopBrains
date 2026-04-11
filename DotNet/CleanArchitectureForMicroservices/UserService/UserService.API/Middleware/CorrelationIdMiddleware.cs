using System;
using Microsoft.AspNetCore.Http;

namespace UserService.API.Middleware
{
  public class CorrelationIdMiddleware
  {
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      const string correlationIdHeader = "X-Correlation-ID";

      var correlationId = context.Request.Headers.TryGetValue(correlationIdHeader, out var values)
          ? values.First()
          : Guid.NewGuid().ToString();

      context.Items["CorrelationId"] = correlationId;
      context.Response.Headers.Add(correlationIdHeader, correlationId);

      await _next(context);
    }
  }
}
