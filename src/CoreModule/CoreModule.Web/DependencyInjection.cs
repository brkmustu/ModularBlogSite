using CoreModule.Application;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Web.Services;

namespace CoreModule.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebCore(this IServiceCollection services, IConfiguration configuration)
    {
        /// Web Core layer registrations
        /// 

        services.AddCoreApp(configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
