using Poc.Core.Messaging.MessageContracts;
using Poc.Core.Messaging.Messages;
using Poc.Core.Messaging.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Core.Messaging
{
    /// <summary>
    /// responsável por criar vários tipos de mensagem
    /// </summary>
    public class MessageFactory
    {
        public static Message<T> Build<T>(T body) => new Message<T> { Body = body };

        public static MessageWithAcknowledge<T> Build<T>(T body, string acknowledgeTopic) =>
            new MessageWithAcknowledge<T>
            {
                Body = body,
                AcknowledgeRequested = true,
                AcknowledgeTopic = acknowledgeTopic
            };

        public static FullMessage<T> Build<T>(T body,
            string eventName,
            bool acknowledgeRequested = false,
            string acknowledgeTopic = null)
        {
            if (acknowledgeRequested == true && string.IsNullOrEmpty(acknowledgeTopic))
            { throw new ArgumentNullException("quando `knowledgeRequested` for 'true', você deve fornecer `knowledgeTopic`"); }

            return new FullMessage<T>
            {
                Body = body,
                EventName = eventName,
                AcknowledgeRequested = acknowledgeRequested,
                AcknowledgeTopic = acknowledgeTopic
            };
        }

        public static AcknowledgeMessage BuildAcknowledge(string id, Result result) =>
          new AcknowledgeMessage
          {
              Id = id,
              Result = result
          };
    }
}
