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
using Poc.Core.Kafka.SubscriberHandlers;
using Poc.Core.Messaging;

namespace Poc.Consumer
{
    public class Startup
    {
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

            //var registeredTopics = services.RegisterMessageHandlers(typeof(ConsumerGeneric).Assembly);
            //services.AddNewHostedSubscriber(registeredTopics);

            services.AddHostedSubscriber();

            // --------------<( string handler)>---------------------------- 
            services.AddSingleton<ISubscriberHandler, StringSubscriberHandler>();
            services.AddSingleton(_ =>
               new SubscriberBinding()
                   .RegisterTopicHandler<ConsumerBasic>("poc-kafka"));
            services.AddScoped<ConsumerBasic>();
            // -----------------------------------------------------------------
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
