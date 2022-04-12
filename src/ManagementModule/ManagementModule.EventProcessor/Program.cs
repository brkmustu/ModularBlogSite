using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ManagementModule.EventProcessor;

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddSerilog();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<SyncManagementDbPermissionWorker>();
        services.AddHostedService<UserRegisteredEventWorker>();
        services.RegisterConfigurationServices(hostContext);
        services.RegisterQueueServices(hostContext);
        services.RegisterRepositoryServices(hostContext);
    })
    .UseSerilog((ctx, lc) => { lc.ReadFrom.Configuration(ctx.Configuration); })
    .Build()
    .Run();