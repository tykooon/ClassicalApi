namespace ClassicalApi.Blazor.Middleware;

public static class MiddlewareExtensios
{
    public static IApplicationBuilder UseLogHeaders(this IApplicationBuilder builder, bool isActive) =>
        isActive ? builder.UseMiddleware<LogHeadersMiddleware>() : builder;

    public static IApplicationBuilder UseHttpsScheme(this IApplicationBuilder builder, bool isActive)
    {
        if (isActive)
        {
            builder.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });
        }
        return builder;
    }

}
