using MassTransit;
using TaxDocumentProcessor.Application.Interfaces;

namespace TaxDocumentProcessor.Infrastructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
        {
            await _publishEndpoint.Publish(message, ct);
        }
    }
}
