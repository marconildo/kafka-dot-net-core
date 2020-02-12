using Poc.Core.Messaging.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Consumer.Subscribers
{
    public class ConsumerBasic : IStringMessageHandler
    {
        public Task Handle(string message, CancellationToken token)
        {
            Console.WriteLine($"Mensagem Recebida {message }");

            return Task.CompletedTask;
        }
    }
}
