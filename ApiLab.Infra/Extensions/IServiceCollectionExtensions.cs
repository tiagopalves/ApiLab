using ApiLab.CrossCutting.Configurations;
using ApiLab.Infra.Repository;
using ApiLab.Infra.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace ApiLab.Infra.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisConnection(this IServiceCollection services, RedisConfiguration? redisConfiguration)
        {
            if (redisConfiguration is not null && !string.IsNullOrEmpty(redisConfiguration.ConnectionString))
            {
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
            }
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IClienteRepository, ClienteRedisRepository>();
            return services;
        }
    }
}
