using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poc.Core.Kafka;
using Poc.Core.Messaging;
using Poc.Core.Messaging.MessageContracts;
using Poc.Core.Messaging.Messages;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Poc.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            var serviceProvider = new ServiceCollection()
               .AddLogging(opt =>
               {
                   opt.AddConsole();
               })
               .Configure<LoggerFilterOptions>(cfg =>
               {
                   cfg.MinLevel = LogLevel.Trace;
               })
               .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogDebug("Starting application");

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var producerConfig = config.GetSection("KafkaConfig").Get<ProducerConfig>();

            IPublisher publisher = new KafkaProducer(producerConfig, serviceProvider.GetService<ILogger<KafkaProducer>>());

            while (true)
            {
                var message = Console.ReadLine();

                //var typedMessage = new MessageWithAcknowledge<Foo>
                //{
                //    Body = new Foo { Bar = message },
                //    AcknowledgeRequested = true,
                //    AcknowledgeTopic = "poc-acknowledge"
                //};

                var typedMessage = new Foo
                {
                    Bar = message
                };

                //await publisher.PublishAsync("poc-kafka", MessageFactory.Build(typedMessage));
                //await publisher.PublishAsync("poc-kafka", MessageFactory.Build(typedMessage, "poc-acknowledge"));
                await publisher.PublishAsync("poc-kafka", message);
            }
        }
    }
}
