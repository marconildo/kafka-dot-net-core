using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Messaging.Handlers
{
    public interface IJObjectMessageHandler : IMessageHandler
    {
        Task Handle(JObject message, CancellationToken token);
    }
}
