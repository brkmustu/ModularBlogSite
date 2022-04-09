using CoreModule.Application.Common.RabbitMqExtensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserPortalModule.Consumers;

public static class QueueExtension
{
    public static IServiceCollection RegisterQueueServices(this IServiceCollection services, HostBuilderContext context)
    {
        var section = context.Configuration.GetSection(RabbitMqOptions.SectionName);
        var options = section.Get<RabbitMqOptions>();

        services.AddMassTransit(c =>
        {
            c.AddConsumer<UserApprovedEventConsumer>();
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
