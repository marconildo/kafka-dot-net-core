using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poc.Core.Kafka.Handlers;
using Poc.Core.Messaging;
using Poc.Core.Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Poc.Core.Kafka.DependencyInjection
{
    public static class KafkaSubscriberExtensions
    {

        public static IServiceCollection AddNewHostedSubscriber(this IServiceCollection services, params string[] topics)
            => AddNewHostedSubscriber(services, new List<string>(topics));


        public static IServiceCollection AddNewHostedSubscriber(this IServiceCollection services, IEnumerable<string> topics)
        {
            return services.AddTransient<IHostedService>(provider =>
            {
                var config = provider.GetRequiredService<ConsumerConfig>();

                if (topics == null)
                { throw new ArgumentNullException(nameof(topics)); }

                return new KafkaSubscriber(
                    provider.GetRequiredService<ILoggerFactory>(),
                    config,
                    provider.GetRequiredService<ISubscriberHandler>(),
                    topics);
            });
        }

        public static IList<string> AddHostedSubscriber(this IServiceCollection services)
        {
            IList<string> topics = null;

            services.AddTransient<IHostedService>(provider =>
            {
                var config = provider.GetRequiredService<ConsumerConfig>();

                var subscriberBinding = provider.GetRequiredService<SubscriberBinding>();
                topics = subscriberBinding.RegisteredTopics;

                if (topics == null)
                { throw new ArgumentNullException(nameof(topics)); }

                return new KafkaSubscriber(
                    provider.GetRequiredService<ILoggerFactory>(),
                    config,
                    provider.GetRequiredService<ISubscriberHandler>(),
                    topics);
            });

            return topics;
        }

        public static IList<string> RegisterMessageHandlers<TMessageHandler>(this IServiceCollection services)
            where TMessageHandler : IMessageHandler
            => RegisterMessageHandlers(services, typeof(TMessageHandler).Assembly);

        public static IList<string> RegisterMessageHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var registeredTopics = new List<string>();
            var subscriberBinding = new SubscriberBinding();

            var types = KafkaHelper.GetClassesImplementingAnInterface(assemblies, typeof(IMessageHandler<>));

            foreach (var messageHandlerType in types)
            {
                var messageType = messageHandlerType.GetInterface(typeof(IMessageHandler<>).Name).GetGenericArguments()[0];

                var topics = messageHandlerType.GetCustomAttribute<TopicsAttribute>()?.Topics
                    ?? throw new Exception($"{nameof(TopicsAttribute)} does not defined for MessageHandler : {messageHandlerType.Name}");

                subscriberBinding.RegisterTopicHandler(messageHandlerType, messageType, topics);

                services.AddScoped(messageHandlerType);

                registeredTopics.AddRange(topics);
            }

            services.AddSingleton(subscriberBinding);
            services.AddSingleton<ISubscriberHandler, SubscriberHandler>();

            return registeredTopics;
        }
    }
}
