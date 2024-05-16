namespace ClassicalApi.Blazor;

public class AppSettings
{
    public bool UseHttpsRequestScheme { get; set; }
    public bool ForceSecureCookie { get; set; }
    public bool LoggingHeaders { get; set; }
    public string ApiServiceUrl { get; }
    public string ApiServiceKey { get; set; }

    public AppSettings(IConfiguration configuration)
    {
        UseHttpsRequestScheme = configuration.GetValue<bool>("UseHttpsRequestScheme");
        ForceSecureCookie = configuration.GetValue<bool>("ForceSecureCookie");
        LoggingHeaders = configuration.GetValue<bool>("LoggingHeaders");
        ApiServiceUrl = configuration["ApiService:Url"] ?? "";
        ApiServiceKey = configuration["ApiService:ApiKey"] ?? "";
    }
}