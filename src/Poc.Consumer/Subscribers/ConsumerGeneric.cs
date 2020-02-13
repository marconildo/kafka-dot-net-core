using Poc.Core.Messaging;
using Poc.Core.Messaging.Handlers;
using Poc.Core.Messaging.Messages;
using Poc.Core.Messaging.Structure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Consumer.Subscribers
{
    [Topics("poc-kafka")]
    public class ConsumerGeneric : IMessageHandler<MessageWithAcknowledge<Foo>>
    {
        private readonly IPublisher _publisher;
        public ConsumerGeneric(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(MessageWithAcknowledge<Foo> message, CancellationToken token)
        {
            Console.WriteLine($"Mensagen recebida: <{ message.Body.Bar }>");

            if (message.AcknowledgeRequested)
            {
                if (message.Body.Bar != "error")
                {
                    await _publisher.PublishAsync(message.AcknowledgeTopic,
                        new AcknowledgeMessage
                        {
                            Id = message.Id,
                            Result = Result.Okay("Sucesso")
                        });
                }
                else
                {
                    await _publisher.PublishAsync(message.AcknowledgeTopic,
                        new AcknowledgeMessage
                        {
                            Id = message.Id,
                            Result = Result.Error("Ocorreu um erro.")
                        });
                }
            }

        }
    }
}
