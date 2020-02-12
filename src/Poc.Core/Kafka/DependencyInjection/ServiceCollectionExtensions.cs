using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Poc.Core.Kafka.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static TConfig AddPoco<TConfig>(this IServiceCollection services,
            IConfiguration configuration,
            string key = null) where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var config = new TConfig();
            configuration.Bind(key ?? typeof(TConfig).Name, config);
            services.AddSingleton(config);
            return config;
        }
    }
}
