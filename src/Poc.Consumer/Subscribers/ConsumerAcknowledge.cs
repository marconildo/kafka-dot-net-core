using Poc.Core.Messaging;
using Poc.Core.Messaging.Handlers;
using Poc.Core.Messaging.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Consumer.Subscribers
{
    [Topics("poc-acknowledge")]
    public class ConsumerAcknowledge : IMessageHandler<AcknowledgeMessage>
    {

        public Task Handle(AcknowledgeMessage message, CancellationToken token)
        {

            if (message.Result.Succeed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Mensagen recebida: <{ message.Result.Message }>");
                Console.WriteLine("Completo!!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro recebido: <{ message.Result.Message }>");
                Console.WriteLine("Completo!!");
                Console.ResetColor();
            }

            return Task.CompletedTask;
        }
    }
}
