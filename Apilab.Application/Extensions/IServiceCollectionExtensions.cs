using Apilab.Application.AppServices;
using Apilab.Application.AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Apilab.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClienteService>();
            return services;
        }
    }
}
