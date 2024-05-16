using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace ClassicalApi.Blazor.Middleware
{
    public class LogHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogHeadersMiddleware> _logger;

        public LogHeadersMiddleware(RequestDelegate next, ILogger<LogHeadersMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("RequestUrl :\n{url}", context.Request.GetDisplayUrl());
            var log = new StringBuilder();
            foreach (var header in context.Request.Headers)
            {
                log.AppendLine($"{header.Key}: {header.Value}");
            }

            _logger.LogInformation("Request Headers : \n\t{log}", log.ToString());

            await _next(context);

            log = new StringBuilder();
            foreach (var header in context.Response.Headers)
            {
                log.AppendLine($"{header.Key}: {header.Value}");
            }

            _logger.LogInformation("Response Headers : \n\t{log}", log.ToString());
        }
    }
}
