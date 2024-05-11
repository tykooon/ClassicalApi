using ClassicalApi.Core.Infrastructure;
using ClassicalApi.Host.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassicalApi.Host;

public static class ConfigureServices
{
    public static IServiceCollection AddConfiguredDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString;
        var isDefaultConnection = configuration.GetValue<bool>("UseDefaultConnection");
        if(isDefaultConnection)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
            return services;
        }

        connectionString = configuration.GetConnectionString("MySqlConnection") ?? "";
        var connectionData = configuration
            .GetSection("Connections")
            .GetSection("MySql")
            .GetSection("Admin")
            .Get<MySqlConnectionSettings>();
        connectionString = connectionData?.UpdateConnectionString(connectionString) ?? "";

        services.AddDbContext<AppDbContext>(options =>
           options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        return services;
    }
}
