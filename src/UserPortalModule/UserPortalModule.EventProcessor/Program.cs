using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UserPortalModule.EventProcessor;

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddSerilog();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<SyncUserPortalDbPermissionsWorker>();
        services.AddHostedService<UserApprovedEventWorker>();
        services.RegisterConfigurationServices(hostContext);
        services.RegisterQueueServices(hostContext);
        services.RegisterRepositoryServices(hostContext);
    })
    .UseSerilog((ctx, lc) => { lc.ReadFrom.Configuration(ctx.Configuration); })
    .Build()
    .Run();