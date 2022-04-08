using CoreModule.Application.Common.RabbitMqExtensions;
using MassTransit;
using UserPortalModule.Consumers;

public static class QueueExtensions
{
    public static IServiceCollection RegisterQueueServices(this IServiceCollection services, IConfiguration section)
    {
        var appSettingsSection = section.GetSection(RabbitMqOptions.SectionName);
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

        //services.AddMassTransit(c =>
        //{
        //    c.AddConsumer<UserApprovedEventConsumer>();
        //});

        //services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
        //{
        //    cfg.Host(options.HostName, options.VirtualHost,
        //        h =>
        //        {
        //            h.Username(options.UserName);
        //            h.Password(options.Password);
        //        });

        //    cfg.ReceiveEndpoint(RabbitMqConsts.ManagementModuleQueueName, e =>
        //    {
        //        e.UseCircuitBreaker(cb =>
        //        {
        //            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
        //            cb.TripThreshold = 15;
        //            cb.ActiveThreshold = 10;
        //            cb.ResetInterval = TimeSpan.FromMinutes(5);
        //        });
        //    });
        //}));

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
