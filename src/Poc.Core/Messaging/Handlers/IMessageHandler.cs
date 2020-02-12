using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Messaging.Handlers
{
    public interface IMessageHandler
    {

    }

    public interface IMessageHandler<TMessage> : IMessageHandler
    {
        Task Handle(TMessage message, CancellationToken token);
    }
}
