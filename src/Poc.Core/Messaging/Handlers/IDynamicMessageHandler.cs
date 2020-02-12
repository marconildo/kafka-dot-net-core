using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Messaging.Handlers
{
    public interface IDynamicMessageHandler : IMessageHandler
    {
        Task Handle(dynamic message, CancellationToken token);
    }
}
