namespace ClassicalApi.Host.Authentication;

public class ApiKeyEndpointFilter(IConfiguration configuration) : IEndpointFilter
{
    private IConfiguration _configuration = configuration;

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if(!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKetHeaderName, out var expectedApiKey))
        {
            return new UnauthorizedResult("Api Key is missing");
        }

        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
        if(apiKey == null || !apiKey.Equals(expectedApiKey))
        {
            return new UnauthorizedResult("Api Key is not valid");
        }

        return await next(context);
    }
}
