using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

namespace Poc.Core.Kafka.Handlers
{
    public interface ISubscriberHandler
    {
        Task HandleMessage(ConsumeResult<Ignore, string> result, CancellationToken token);
    }
}
