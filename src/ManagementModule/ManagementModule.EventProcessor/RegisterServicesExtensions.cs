using CoreModule.Application;
using CoreModule.Application.Common.RabbitMqExtensions;
using ManagementModule;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class RegisterServicesExtensions
{
    public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services, HostBuilderContext context)
    {
        services.AddApplication(context.Configuration)
            .AddPersistence(context.Configuration);

        return services;
    }

    public static IServiceCollection RegisterConfigurationServices(this IServiceCollection services, HostBuilderContext context)
    {
        services.Configure<RabbitMqOptions>(context.Configuration.GetSection(RabbitMqOptions.SectionName));

        return services;
    }
}
