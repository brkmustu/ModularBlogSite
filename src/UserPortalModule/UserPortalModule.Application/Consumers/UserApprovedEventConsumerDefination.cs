using CoreModule.Application.Common.RabbitMqExtensions;
using MassTransit;

namespace UserPortalModule.Consumers;

public class UserApprovedEventConsumerDefination : ConsumerDefinition<UserApprovedEventConsumer>
{
    public UserApprovedEventConsumerDefination()
    {
        EndpointName = RabbitMqConsts.UserPortalModuleQueueName;

        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<UserApprovedEventConsumer> consumerConfigurator
        )
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
