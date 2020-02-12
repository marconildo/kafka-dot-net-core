namespace Poc.Core.Messaging.MessageContracts
{
    public interface IEventMessage : IMessage
    {
        string EventName { get; set; }
    }
}
