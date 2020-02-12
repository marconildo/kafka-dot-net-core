using Poc.Core.Messaging.MessageContracts;

namespace Poc.Core.Messaging.Messages
{
    public class MessageWithAcknowledge<T> : IMessageWithAcknowledge
    {
        public string Id { get; set; }

        public bool AcknowledgeRequested { get; set; }

        public string AcknowledgeTopic { get; set; }

        public T Body { get; set; }
    }
}
