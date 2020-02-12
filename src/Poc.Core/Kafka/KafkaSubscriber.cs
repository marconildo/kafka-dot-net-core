using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poc.Core.Kafka.SubscriberHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Core.Kafka
{
    public class KafkaSubscriber : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ConsumerConfig _config;
        private readonly ISubscriberHandler _handler;
        private readonly IEnumerable<string> _topics;

        public KafkaSubscriber(ILoggerFactory loggerFactory,
            ConsumerConfig config,
            ISubscriberHandler handler,
            IEnumerable<string> topics)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger(nameof(KafkaSubscriber) + "-" + config.GroupId);
            _handler = handler;
            _topics = topics;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = new ConsumerBuilder<Ignore, string>(_config)
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    _logger.LogInformation($"Assigned partitions: [{string.Join(", ", partitions)}]");
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    _logger.LogInformation($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .Build();

            consumer.Subscribe(_topics);

            Task.Run(async () => await Consume(consumer, cancellationToken), cancellationToken);

            return Task.CompletedTask;
        }

        private async Task Consume(IConsumer<Ignore, string> consumer, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        if (consumeResult.IsPartitionEOF)
                        {
                            _logger.LogTrace(
                                $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                            continue;
                        }

                        await _handler.HandleMessage(consumeResult, cancellationToken);

                        if (true)
                        { 
                            try
                            {
                                consumer.Commit(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                _logger.LogError($"Commit error: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consume error: {e.Error.Reason}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Error occurred on consumer handler Method.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Closing consumer.");
                consumer.Close();
            }
        }
    }
}
