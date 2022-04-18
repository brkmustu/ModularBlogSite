using Consul;
using CoreModule;

public static class ConsulBootstrapper
{
    public static IServiceCollection AddConsuleClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(x => new ConsulClient(consulConfig =>
        {
            var address = CommonSettings.GetConsulAddress(configuration);
            consulConfig.Address = new Uri(address);
        }));

        return services;
    }

    public static IApplicationBuilder RegisterWithConsule(this IApplicationBuilder app, ICollection<string> urls)
    {
        var consuleClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var appGuid = app.ApplicationServices.GetRequiredService<IAppGuid>();

        var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

        var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

        var serviceName = CommonSettings.GetEnvironmentServiceName();
        var servicePort = CommonSettings.GetEnvironmentServicePort();

        var address = serviceName.IsNullOrEmpty() ? urls.First() : "http://" + serviceName + ":" + servicePort;

        var uri = new Uri(address);
        var registration = new AgentServiceRegistration
        {
            ID = $"{uri.Host}:{appGuid.AppId}",
            Name = $"ManagementService",
            Address = $"{uri.Host}",
            Port = uri.Port,
            Tags = new[] { "Management Service", "Management" }
        };

        logger.LogInformation("Registering with Consul");
        consuleClient.Agent.ServiceDeregister(registration.ID).Wait();
        consuleClient.Agent.ServiceRegister(registration).Wait();

        return app;
    }

    public static IApplicationBuilder DeregisterWithConsule(this IApplicationBuilder app, ICollection<string> urls)
    {
        var consuleClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var appGuid = app.ApplicationServices.GetRequiredService<IAppGuid>();

        var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

        var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

        var serviceName = CommonSettings.GetEnvironmentServiceName();
        var servicePort = CommonSettings.GetEnvironmentServicePort();

        var address = serviceName.IsNullOrEmpty() ? urls.First() : "http://" + serviceName + ":" + servicePort;

        var uri = new Uri(address);
        var registration = new AgentServiceRegistration
        {
            ID = $"{uri.Host}:{appGuid.AppId}",
            Name = $"ManagementService",
            Address = $"{uri.Host}",
            Port = uri.Port,
            Tags = new[] { "Management Service", "Management" }
        };

        logger.LogInformation("Deregistering from Consul");
        consuleClient.Agent.ServiceDeregister(registration.ID).Wait();

        return app;
    }
}
