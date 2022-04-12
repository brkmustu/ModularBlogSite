using CoreModule.Application.Common.RabbitMqExtensions;
using ManagementModule.Consumers;
using MassTransit;

public static class QueueBootstrapper
{
    public static IServiceCollection AddQueueServices(this IServiceCollection services, IConfiguration configuration)
    {
        var options = CommonSettings.GetRabbitMqOptions(configuration);

        services.AddMassTransit(c =>
        {
            c.AddConsumer<UserRegisteredEventConsumer>();
            c.AddConsumer<SyncManagementDbPermissionConsumer>();

            c.UsingRabbitMq((context, config) =>
            {
                config.Host(options.HostName, options.VirtualHost, hc =>
                {
                    hc.Username(options.UserName);
                    hc.Password(options.Password);
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
