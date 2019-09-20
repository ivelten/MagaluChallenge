using Magalu.Challenge.Data.External;
using Magalu.Challenge.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Magalu.Challenge.Data
{
    public static class ServiceCollectionExtetnsions
    {
        public static IServiceCollection ConfigureExternalProductApiRepository(
            this IServiceCollection services)
        {
            return services
                .AddScoped<IExternalProductApiClient, ExternalProductApiClient>()
                .AddScoped<IReadOnlyRepository<Product>, ExternalProductApiRepository>();
        }
    }
}
