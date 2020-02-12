using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Poc.Core.Helpers;
using Poc.Core.Messaging;
using Poc.Core.Messaging.MessageContracts;
using System;
using System.Threading.Tasks;

namespace Poc.Core.Kafka
{
    public class KafkaProducer : IPublisher
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(ProducerConfig config, ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public void Publish(string topic, string message)
        {
            if (message == null) { throw new ArgumentNullException(nameof(message)); }
            if (string.IsNullOrEmpty(topic)) { throw new ArgumentNullException(nameof(topic)); }

            try
            {
                _producer.Produce(
                    topic,
                    new Message<Null, string> { Value = message });
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                throw;
            }
        }

        public void Publish(string topic, object message)
        {
            if (message == null)
            { throw new ArgumentNullException(nameof(message)); }

            Publish(topic, JsonConvert.SerializeObject(message));
        }

        public void Publish(string topic, IMessage message)
        {
            if (message == null)
            { throw new ArgumentNullException(nameof(message)); }

            if (message.Id == null)
            { message.Id = IdGenerator.GetNewId(); }

            Publish(topic, JsonConvert.SerializeObject(message));
        }

        public async Task PublishAsync(string topic, string message)
        {
            if (message == null) { throw new ArgumentNullException(nameof(message)); }
            if (string.IsNullOrEmpty(topic)) { throw new ArgumentNullException(nameof(topic)); }

            try
            {
                var deliveryReport = await _producer.ProduceAsync(
                    topic,
                    new Message<Null, string> { Value = message });

                _logger.LogTrace($"delivered to: {deliveryReport.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                throw;
            }
        }

        public async Task PublishAsync(string topic, object message)
        {
            if (message == null) { throw new ArgumentNullException(nameof(message)); }

            await PublishAsync(topic, JsonConvert.SerializeObject(message));
        }

        public async Task PublishAsync(string topic, IMessage message)
        {
            if (message == null)
            { throw new ArgumentNullException(nameof(message)); }

            if (message.Id == null)
            { message.Id = IdGenerator.GetNewId(); }

            await PublishAsync(topic, JsonConvert.SerializeObject(message));
        }
    }
}
