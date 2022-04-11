using CoreModule.Application.Common.RabbitMqExtensions;
using MassTransit;
using UserPortalModule.Consumers;

public static class QueueBootstrapper
{
    public static IServiceCollection AddQueueServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection(RabbitMqOptions.SectionName);
        var options = appSettingsSection.Get<RabbitMqOptions>();

        services.AddMassTransit(c =>
        {
            c.AddConsumer<UserApprovedEventConsumer>();

            c.UsingRabbitMq((context, config) =>
            {
                config.Host(options.HostName, options.VirtualHost, hc =>
                {
                    hc.Username(options.UserName);
                    hc.Password(options.Password);
                });

                config.ReceiveEndpoint(RabbitMqConsts.UserPortalModuleQueueName, e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });
                });
            });
        });

        services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
        services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
        services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

        services.AddOptions<MassTransitHostOptions>()
            .Configure(options =>
            {
                // if specified, waits until the bus is started before
                // returning from IHostedService.StartAsync
                // default is false
                options.WaitUntilStarted = true;

                // if specified, limits the wait time when starting the bus
                options.StartTimeout = TimeSpan.FromSeconds(10);

                // if specified, limits the wait time when stopping the bus
                options.StopTimeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
