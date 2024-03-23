namespace ClassicalApi.Host.Authentication;

public class UnauthorizedResult(object body) : IResult, IStatusCodeHttpResult
{
    private readonly object _body = body;

    public int? StatusCode => StatusCodes.Status401Unauthorized;

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.StatusCode = StatusCode!.Value;
        if(_body is string s)
        {
            await httpContext.Response.WriteAsync(s);
            return;
        }

        await httpContext.Response.WriteAsJsonAsync(_body);
    }
}
