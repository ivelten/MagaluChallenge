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
                var products = new Product[]
                {
                    new Product { Title = "Beer (500ml)", Brand = "Heineken", Image = "http://magalu.com/product/image/1", Price = 10.2M },
                    new Product { Title = "Beer (450ml)", Brand = "Budweiser", Image = "http://magalu.com/product/image/2", Price = 3.4M },
                    new Product { Title = "LED TV 59 Inch", Brand = "LG", Image = "http://magalu.com/product/image/3", Price = 2890.56M },
                    new Product { Title = "Coffee Machine", Brand = "Drive Cofee Inc", Image = "http://magalu.com/product/image/4", Price = 200.5M },
                    new Product { Title = "Coffee", Brand = "Dolce Gusto", Image = "http://magalu.com/product/image/5", Price = 200.5M },
                    new Product { Title = "Air Frier", Brand = "Panasonic", Image = "http://magalu.com/product/image/6", Price = 200.5M }
                };

                context.Products.AddRange(products);

                var customers = new Customer[]
                {
                    new Customer { Name = "John Doe", Email = "john.doe367@someprovider.com" },
                    new Customer { Name = "Mary Dias", Email = "mari-dias@myemail.com" },
                    new Customer { Name = "Leo Mango", Email = "leo.mango@another-provider.com" },
                    new Customer { Name = "Mary Cell", Email = "mary.cell@new-isp.com" },
                    new Customer { Name = "Squall Leonheart", Email = "squall_leonheart@square-enix.com" },
                    new Customer { Name = "Alice Farias", Email = "alice.farias@older.isp.com" }
                };

                context.Customers.AddRange(customers);

                var productReviews = new ProductReview[]
                {
                    new ProductReview { CustomerId = customers[0].Id, ProductId = products[0].Id, Score = 4.1f },
                    new ProductReview { CustomerId = customers[0].Id, ProductId = products[1].Id, Score = 3.0f },
                    new ProductReview { CustomerId = customers[0].Id, ProductId = products[2].Id, Score = 5.0f, Comments = "Nice image quality, durable and easy to operate." },
                    new ProductReview { CustomerId = customers[0].Id, ProductId = products[3].Id, Score = 2.9f },
                    new ProductReview { CustomerId = customers[0].Id, ProductId = products[4].Id, Score = 4.0f },
                    new ProductReview { CustomerId = customers[0].Id, ProductId = products[5].Id, Score = 1.7f },
                    new ProductReview { CustomerId = customers[1].Id, ProductId = products[0].Id, Score = 3.5f },
                    new ProductReview { CustomerId = customers[1].Id, ProductId = products[1].Id, Score = 4.6f },
                    new ProductReview { CustomerId = customers[2].Id, ProductId = products[2].Id, Score = 4.8f },
                    new ProductReview { CustomerId = customers[1].Id, ProductId = products[3].Id, Score = 3.1f },
                    new ProductReview { CustomerId = customers[4].Id, ProductId = products[1].Id, Score = 5.0f },
                    new ProductReview { CustomerId = customers[5].Id, ProductId = products[3].Id, Score = 4.0f }
                };

                context.ProductReviews.AddRange(productReviews);

                var favoriteProducts = new FavoriteProduct[]
                {
                    new FavoriteProduct { CustomerId = customers[0].Id, ProductId = products[2].Id },
                    new FavoriteProduct { CustomerId = customers[2].Id, ProductId = products[2].Id },
                    new FavoriteProduct { CustomerId = customers[4].Id, ProductId = products[1].Id },
                    new FavoriteProduct { CustomerId = customers[5].Id, ProductId = products[3].Id }
                };

                context.FavoriteProducts.AddRange(favoriteProducts);

                var users = new User[]
                {
                    new User { Username = "user", PasswordHash = hashingService.HashPassword("user"), Role = Roles.User },
                    new User { Username = "admin", PasswordHash = hashingService.HashPassword("admin"), Role = Roles.Administrator },
                    new User { Username = "john", PasswordHash = hashingService.HashPassword("john"), Role = Roles.User, CustomerId = 1 }
                };

                context.Users.AddRange(users);

                context.SaveChanges();
            }
        }
    }
}
