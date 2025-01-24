using ApiLab.CrossCutting.Issuer.Interfaces;
using ApiLab.CrossCutting.LogManager.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.LogManager.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingService(this IServiceCollection services)
        {
            //TODO: Verificar o melhor local para essas injeções
            services.AddSingleton<IIssuer, Issuer.Issuer>();
            services.AddSingleton<ILogManager, LogManager>();

            services.AddSingleton<ILogService, LogService>();

            return services;
        }
    }
}