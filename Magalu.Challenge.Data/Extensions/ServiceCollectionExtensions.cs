using Magalu.Challenge.Domain.Entities;
using Magalu.Challenge.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Magalu.Challenge.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMagaluRepositories(this IServiceCollection services, string connectionString)
        {
            return services
                .AddScoped<DbContext, MagaluContext>()
                .AddDbContext<MagaluContext>(options =>
            {
                options.UseMySql(connectionString);
            })
                .AddUnitOfWork<MagaluContext>()
                .AddCustomRepository<Customer, Repository<Customer>>()
                .AddCustomRepository<Product, Repository<Product>>()
                .AddCustomRepository<ProductReview, Repository<ProductReview>>()
                .AddCustomRepository<FavoriteProduct, Repository<FavoriteProduct>>()
                .AddCustomRepository<User, Repository<User>>()
                .AddCustomRepository<RequestResponseLog, Repository<RequestResponseLog>>();
        }
    }
}
