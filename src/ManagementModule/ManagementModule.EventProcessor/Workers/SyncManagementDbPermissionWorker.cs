using CoreModule.Application.Common.RabbitMqExtensions;
using ManagementModule.Consumers;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ManagementModule.EventProcessor;

public class SyncManagementDbPermissionWorker : BackgroundService
{
    private readonly ILogger<SyncManagementDbPermissionWorker> _logger;
    private readonly IBusControl _busControl;
    private readonly IServiceProvider _serviceProvider;

    public SyncManagementDbPermissionWorker(ILogger<SyncManagementDbPermissionWorker> logger, IBusControl busControl, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _busControl = busControl;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("SyncManagementDbPermissionConsumer started");

            var hostReceiveEndpointHandler = _busControl.ConnectReceiveEndpoint(RabbitMqConsts.ManagementModule_SyncPermission_QueueName, x =>
            {
                x.Consumer<SyncManagementDbPermissionConsumer>(_serviceProvider);
            });

            await hostReceiveEndpointHandler.Ready;
        }
        catch (Exception ex)
        {
            _logger.LogError("SyncManagementDbPermissionConsumer error", ex);
        }
    }
}
