using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Kafka.Contracts
{
    public interface IJObjectMessageHandler
    {
        Task Handle(JObject message, CancellationToken token);
    }
}
