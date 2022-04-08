using CoreModule.Application.Common.RabbitMqExtensions;
using MassTransit;
using UserPortalModule.Consumers;

namespace UserPortalModule.WebApi.Workers;

public class UserApprovedEventWorker : BackgroundService
{
    private readonly ILogger<UserApprovedEventWorker> _logger;
    private readonly IBusControl _busControl;
    private readonly IServiceProvider _serviceProvider;

    public UserApprovedEventWorker(ILogger<UserApprovedEventWorker> logger, IBusControl busControl, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _busControl = busControl;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("UserApprovedEventConsumer started");

            var hostReceiveEndpointHandler = _busControl.ConnectReceiveEndpoint(RabbitMqConsts.UserPortalModuleQueueName, x =>
            {
                x.Consumer<UserApprovedEventConsumer>(_serviceProvider);
            });

            await hostReceiveEndpointHandler.Ready;
        }
        catch (Exception ex)
        {
            _logger.LogError("UserApprovedEventConsumer error", ex);
        }
    }
}
