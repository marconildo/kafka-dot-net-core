using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

namespace Poc.Core.Kafka.SubscriberHandlers
{
    public interface ISubscriberHandler
    {
        Task HandleMessage(ConsumeResult<Ignore, string> result, CancellationToken token);
    }
}
