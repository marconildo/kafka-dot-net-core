using Poc.Core.Messaging.Structure;

namespace Poc.Core.Messaging.Messages
{
    public class AcknowledgeMessage
    {
        public string Id { get; set; }

        public Result Result { get; set; }
    }
}
