using Magalu.Challenge.ApplicationServices;
using Microsoft.Extensions.DependencyInjection;

namespace Magalu.Challenge.Data.Development
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDevelopmentDatabase(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var context = serviceProvider.GetService<MagaluContext>();
            var hashingService = serviceProvider.GetService<IHashingService>();

            var databaseInitializer = new MagaluContextDatabaseInitializer(context, hashingService);

            databaseInitializer.InitializeDevelopmentEnvironmentDatabase();

            return services;
        }
    }
}
