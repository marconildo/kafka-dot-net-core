using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Messaging.Handlers
{
    public interface IStringMessageHandler : IMessageHandler
    {
        Task Handle(string message, CancellationToken token);
    }
}
