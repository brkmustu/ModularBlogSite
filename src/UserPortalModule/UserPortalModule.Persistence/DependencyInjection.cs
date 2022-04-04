using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserPortalModule.Common;

namespace UserPortalModule;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        /// persistence layer registrations
        /// 

        string connectionString = configuration.GetDefaultConnectionString();

        services.AddDbContext<UserPortalModuleDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUserPortalModuleDbContext>(provider => provider.GetService<UserPortalModuleDbContext>());

        return services;
    }
    public static string GetDefaultConnectionString(this IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_UserPortalModule");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = configuration.GetConnectionString("UserPortalModule");
        }
        return connectionString;
    }
}

