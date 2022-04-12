using CoreModule.Application.Common.RabbitMqExtensions;
using UserPortalModule.Consumers;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UserPortalModule.EventProcessor;

public class SyncUserPortalDbPermissionsWorker : BackgroundService
{
    private readonly ILogger<SyncUserPortalDbPermissionsWorker> _logger;
    private readonly IBusControl _busControl;
    private readonly IServiceProvider _serviceProvider;

    public SyncUserPortalDbPermissionsWorker(ILogger<SyncUserPortalDbPermissionsWorker> logger, IBusControl busControl, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _busControl = busControl;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("SyncUserPortalDbPermissionsWorker started");

            var hostReceiveEndpointHandler = _busControl.ConnectReceiveEndpoint(RabbitMqConsts.UserPortalModule_SyncPermission_QueueName, x =>
            {
                x.Consumer<SyncUserPortalDbPermissionsConsumer>(_serviceProvider);
            });

            await hostReceiveEndpointHandler.Ready;
        }
        catch (Exception ex)
        {
            _logger.LogError("SyncUserPortalDbPermissionsWorker error", ex);
        }
    }
}
