using CoreModule.Application.Common.RabbitMqExtensions;
using MassTransit;

namespace ManagementModule.Consumers;

public class UserRegisteredEventConsumerDefination : ConsumerDefinition<UserRegisteredEventConsumer>
{
    public UserRegisteredEventConsumerDefination()
    {
        EndpointName = RabbitMqConsts.ManagementModuleQueueName;

        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator, 
            IConsumerConfigurator<UserRegisteredEventConsumer> consumerConfigurator
        )
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
