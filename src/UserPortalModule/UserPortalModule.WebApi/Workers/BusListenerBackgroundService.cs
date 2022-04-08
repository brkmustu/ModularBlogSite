using MassTransit;

namespace UserPortalModule.WebApi.Workers;

public class BusListenerBackgroundService : BackgroundService
{
    private readonly IBusControl _busControl;

    public BusListenerBackgroundService(IBusControl busControl, ILogger<BusListenerBackgroundService> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }

    private readonly ILogger<BusListenerBackgroundService> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _busControl.StartAsync(stoppingToken);
            _logger.LogInformation("Bus listener started!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserPortalService : BusListenerBackgroundService");
            throw;
        }
    }
}
