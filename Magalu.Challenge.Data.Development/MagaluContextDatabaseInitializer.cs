using Magalu.Challenge.ApplicationServices;
using Magalu.Challenge.Domain;
using Magalu.Challenge.Domain.Entities;
using System;
using System.Linq;

namespace Magalu.Challenge.Data.Development
{
    public sealed class MagaluContextDatabaseInitializer
    {
        private readonly MagaluContext context;

        private readonly IHashingService hashingService;

        public MagaluContextDatabaseInitializer(MagaluContext context, IHashingService hashingService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        public void InitializeDevelopmentEnvironmentDatabase()
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Products.Any() && !context.Customers.Any() && !context.Users.Any())
            {
                context.Products.AddRange(DatabaseSeeds.Products);

                context.Customers.AddRange(DatabaseSeeds.Customers);

                context.ProductReviews.AddRange(DatabaseSeeds.ProductReviews);

                context.FavoriteProducts.AddRange(DatabaseSeeds.FavoriteProducts);

                var users = new User[]
                {
                    new User { Username = DatabaseSeeds.UserUsername, PasswordHash = hashingService.HashPassword(DatabaseSeeds.UserPassword), Role = Roles.User },
                    new User { Username = DatabaseSeeds.AdminUsername, PasswordHash = hashingService.HashPassword(DatabaseSeeds.AdminPassword), Role = Roles.Administrator },
                    new User { Username = DatabaseSeeds.CustomerUsername, PasswordHash = hashingService.HashPassword(DatabaseSeeds.CustomerPassword), Role = Roles.User, CustomerId = DatabaseSeeds.Customers[0].Id }
                };

                context.Users.AddRange(users);

                context.SaveChanges();
            }
        }
    }
}
