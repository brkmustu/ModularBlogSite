using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ManagementModule.Common;

namespace ManagementModule;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        /// persistence layer registrations
        /// 

        string connectionString = configuration.GetDefaultConnectionString();

        services.AddDbContext<ManagementModuleDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IManagementModuleDbContext>(provider => provider.GetService<ManagementModuleDbContext>());

        return services;
    }
    public static string GetDefaultConnectionString(this IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_ManagementModule");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = configuration.GetConnectionString("ManagementModule");
        }
        return connectionString;
    }
}

