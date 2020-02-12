using Poc.Core.Messaging.MessageContracts;

namespace Poc.Core.Messaging.Messages
{
    public class Message<T> : IMessage
    {
        public string Id { get; set; }
        public T Body { get; set; }
    }
}
