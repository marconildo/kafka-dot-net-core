using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Consumer.Subscribers;
using Poc.Core.Kafka;
using Poc.Core.Kafka.DependencyInjection;
using Poc.Core.Messaging;

namespace Poc.Consumer
{
    public class Startup
    {
        public const string PocTopic = "poc-kafka";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPoco<ConsumerConfig>(Configuration, "KafkaConfig");
            services.AddPoco<ProducerConfig>(Configuration, "KafkaConfig");

            services.AddScoped<IPublisher, KafkaProducer>();

            var registeredTopics = services.RegisterMessageHandlers(typeof(ConsumerGeneric).Assembly);

            services.AddNewHostedSubscriber(registeredTopics);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
