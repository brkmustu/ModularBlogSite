using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurveyModule.Common;

namespace SurveyModule;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        /// persistence layer registrations
        /// 

        string connectionString = configuration.GetDefaultConnectionString();

        services.AddDbContext<SurveyModuleDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<ISurveyModuleDbContext>(provider => provider.GetService<SurveyModuleDbContext>());

        return services;
    }
    public static string GetDefaultConnectionString(this IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_SurveyModule");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = configuration.GetConnectionString("SurveyModule");
        }
        return connectionString;
    }
}

