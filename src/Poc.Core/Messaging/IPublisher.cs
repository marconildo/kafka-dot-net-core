using Poc.Core.Messaging.MessageContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Core.Messaging
{
    public interface IPublisher
    {
        Task PublishAsync(string topic, string message);
        Task PublishAsync(string topic, object message);
        Task PublishAsync(string topic, IMessage message);

        void Publish(string topic, string message);
        void Publish(string topic, object message);
        void Publish(string topic, IMessage message);
    }
}
