using Poc.Core.Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poc.Core.Kafka.DependencyInjection
{
    public class SubscriberBinding
    {
        public IList<string> RegisteredTopics
        {
            get
            {
                var topics = new List<string>();

                topics.AddRange(TopicHandlers.Keys);
                topics.AddRange(GenericTopicHandlers.Keys);

                return topics;
            }
        }

        public IDictionary<string, Type> TopicHandlers { get; private set; } = new Dictionary<string, Type>();
        public IDictionary<string, KeyValuePair<Type, Type>> GenericTopicHandlers { get; internal set; }
            = new Dictionary<string, KeyValuePair<Type, Type>>();

        public SubscriberBinding RegisterTopicHandler<TSubscriber>(string topic)
        {
            if (string.IsNullOrEmpty(topic)) { throw new ArgumentNullException(nameof(topic)); }

            TopicHandlers[topic] = typeof(TSubscriber);

            return this;
        }

        public SubscriberBinding RegisterTopicHandler<TMessageHandler, TMessage>(params string[] topics)
            where TMessageHandler : IMessageHandler<TMessage>
        {
            if (topics == null || !topics.Any())
            { throw new ArgumentNullException("Topicos não foram fornecidos para o MessageHandler : " + typeof(TMessageHandler).Name); }

            foreach (var topic in topics)
            {
                GenericTopicHandlers[topic] = new KeyValuePair<Type, Type>(typeof(TMessageHandler), typeof(TMessage));
            }

            return this;
        }

        public SubscriberBinding RegisterTopicHandler(Type typeMessageHandler, Type typeMessage, params string[] topics)
        {
            if (topics == null || !topics.Any())
            { throw new ArgumentNullException("Topicos não foram fornecidos para o MessageHandler : " + typeMessageHandler.Name); }

            foreach (var topic in topics)
            {
                GenericTopicHandlers[topic] = new KeyValuePair<Type, Type>(typeMessageHandler, typeMessage);
            }

            return this;
        }
    }
}
