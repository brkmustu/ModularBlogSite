using Consul;

public static class ConsulBootstrapper
{
    public static IServiceCollection AddConsuleClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(x => new ConsulClient(consulConfig =>
        {
            var address = configuration["ConsulConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));

        return services;
    }

    public static IApplicationBuilder RegisterWithConsule(this IApplicationBuilder app, ICollection<string> urls)
    {
        var consuleClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

        var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

        var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

        var address = urls.First();

        var uri = new Uri(address);
        var registration = new AgentServiceRegistration
        {
            ID = $"UserPortalService",
            Name = $"UserPortalService",
            Address = $"{uri.Host}",
            Port = uri.Port,
            Tags = new[] { "User Portal Service", "Portal" }
        };

        logger.LogInformation("Registering with Consul");
        consuleClient.Agent.ServiceDeregister(registration.ID).Wait();
        consuleClient.Agent.ServiceRegister(registration).Wait();

        return app;
    }

    public static IApplicationBuilder DeregisterWithConsule(this IApplicationBuilder app, ICollection<string> urls)
    {
        var consuleClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

        var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

        var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

        var address = urls.First();

        var uri = new Uri(address);
        var registration = new AgentServiceRegistration
        {
            ID = $"UserPortalService",
            Name = $"UserPortalService",
            Address = $"{uri.Host}",
            Port = uri.Port,
            Tags = new[] { "User Portal Service", "Portal" }
        };

        logger.LogInformation("Deregistering from Consul");
        consuleClient.Agent.ServiceDeregister(registration.ID).Wait();

        return app;
    }
}
