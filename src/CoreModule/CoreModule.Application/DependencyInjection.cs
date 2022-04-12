using CoreModule.Application.Common;
using CoreModule.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreModule.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreApp(this IServiceCollection services, IConfiguration configuration)
    {
        /// Web Core layer registrations
        /// 

        services.Configure<SystemOptions>(configuration.GetSection(SystemOptions.SectionName));

        services.AddSingleton<IDateTime, MachineDateTime>();

        return services;
    }
}
