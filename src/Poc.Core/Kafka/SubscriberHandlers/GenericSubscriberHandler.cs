using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Poc.Core.Kafka.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Kafka.SubscriberHandlers
{
    public class GenericSubscriberHandler : ISubscriberHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly SubscriberBinding _binding;
        private readonly ConsumerConfig _config;

        public GenericSubscriberHandler(IServiceProvider serviceProvider,
            ILogger<ISubscriberHandler> logger,
            ConsumerConfig config,
            SubscriberBinding binding)
        {
            _config = config;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _binding = binding;
        }

        public async Task HandleMessage(ConsumeResult<Ignore, string> result, CancellationToken token)
        {
            var isTopicHandlerAvailable = _binding.GenericTopicHandlers.TryGetValue(result.Topic, out var handlerType);
            if (!isTopicHandlerAvailable)
            {
                _logger.LogWarning($"<{_config.GroupId}> received message on topic <{result.Topic}>, but there is no handler registered for topic.");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {

                var handler = this.GetHandler(scope, handlerType.Key);

                _logger.LogTrace($"<{_config.GroupId}> received message on topic <{result.Topic}>");

                dynamic value = JsonConvert.DeserializeObject(result.Value, handlerType.Value);

                await (Task)handler.Handle(value, (dynamic)token);
            }
        }

        private dynamic GetHandler(IServiceScope scope, Type handlerType)
        {
            var handler = scope.ServiceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new NullReferenceException(
                    $"<{_config.GroupId}> exception: no handler found for type <{handlerType}>");
            }

            return handler;
        }
    }
}
