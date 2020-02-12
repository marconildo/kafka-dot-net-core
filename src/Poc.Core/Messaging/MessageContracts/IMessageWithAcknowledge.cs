namespace Poc.Core.Messaging.MessageContracts
{
    /// <summary>
    /// fornece uma mensagem com uma solicitação de confirmação
    /// </summary>
    public interface IMessageWithAcknowledge : IMessage
    {
        /// <summary>
        /// indica se o publisher solicita aprovação
        /// </summary>
        bool AcknowledgeRequested { get; set; }

        /// <summary>
        /// define o tópico que o AcknowledgeMessage deve ser publicado
        /// </summary>
        string AcknowledgeTopic { get; set; }
    }
}
