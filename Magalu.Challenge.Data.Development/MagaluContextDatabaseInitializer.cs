using Magalu.Challenge.Shared.Abstractions;
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
            context.Database.EnsureCreated();

            if (!context.Products.Any() && !context.Customers.Any() && !context.Users.Any())
            {
                context.Products.AddRange(DatabaseSeeds.Products);

                context.Customers.AddRange(DatabaseSeeds.Customers);

                context.ProductReviews.AddRange(DatabaseSeeds.ProductReviews);

                context.FavoriteProducts.AddRange(DatabaseSeeds.FavoriteProducts);

                var users = new User[]
                {
                    new User { Username = "user", PasswordHash = hashingService.HashPassword("userpw"), Role = Roles.User },
                    new User { Username = "admin", PasswordHash = hashingService.HashPassword("adminpw"), Role = Roles.Administrator },
                    new User { Username = "john", PasswordHash = hashingService.HashPassword("johnpw"), Role = Roles.User, CustomerId = 1 }
                };

                context.Users.AddRange(users);

                context.SaveChanges();
            }
        }
    }
}
