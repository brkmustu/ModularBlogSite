using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserPortalModule.Consumers;

public static class QueueExtension
{
    public static IServiceCollection RegisterQueueServices(this IServiceCollection services, HostBuilderContext context)
    {
        var options = CommonSettings.GetRabbitMqOptions(context.Configuration);

        services.AddMassTransit(c =>
        {
            c.AddConsumer<UserApprovedEventConsumer>();
            c.AddConsumer<SyncUserPortalDbPermissionsConsumer>();
        });

        services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(options.HostName, options.VirtualHost, h => {
                h.Username(options.UserName);
                h.Password(options.Password);
            });
        }));

        return services;
    }
}
