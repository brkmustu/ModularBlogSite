using CoreModule.Application.Common.RabbitMqExtensions;
using ManagementModule.Consumers;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ManagementModule.EventProcessor;

public class UserRegisteredEventWorker : BackgroundService
{
    private readonly ILogger<UserRegisteredEventWorker> _logger;
    private readonly IBusControl _busControl;
    private readonly IServiceProvider _serviceProvider;

    public UserRegisteredEventWorker(ILogger<UserRegisteredEventWorker> logger, IBusControl busControl, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _busControl = busControl;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("UserRegisteredEventConsumer started");

            var hostReceiveEndpointHandler = _busControl.ConnectReceiveEndpoint(RabbitMqConsts.ManagementModule_UserRegistration_QueueName, x =>
            {
                x.Consumer<UserRegisteredEventConsumer>(_serviceProvider);
            });

            await hostReceiveEndpointHandler.Ready;
        }
        catch (Exception ex)
        {
            _logger.LogError("UserRegisteredEventConsumer error", ex);
        }
    }
}
