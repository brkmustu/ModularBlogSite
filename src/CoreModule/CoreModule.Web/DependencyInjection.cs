using CoreModule.Application.Common.Interfaces;
using CoreModule.Web.Services;

namespace CoreModule.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebCore(this IServiceCollection container)
    {
        /// application layer registrations
        /// 

        container.AddScoped<ICurrentUserService, CurrentUserService>();

        return container;
    }
}
