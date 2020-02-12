namespace Poc.Core.Messaging.MessageContracts
{
    public class FullMessage<T> : IMessage, IMessageWithAcknowledge, IEventMessage
    {
        public string Id { get; set; }

        public string EventName { get; set; }

        public bool AcknowledgeRequested { get; set; }

        public string AcknowledgeTopic { get; set; }

        public T Body { get; set; }
    }
}
