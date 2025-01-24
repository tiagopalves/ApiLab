using Apilab.Application.AppServices;
using Apilab.Application.AppServices.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ApiLab.Api.Common.IoC
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ILabService, LabService>();

            return services;
        }
    }
}
