using Magalu.Challenge.Domain;
using Magalu.Challenge.Domain.Entities;

namespace Magalu.Challenge.Data.Development
{
    public static class DatabaseSeeds
    {
        public static Product[] Products = new[]
            {
                new Product { Id = 1, Title = "Beer (500ml)", Brand = "Heineken", Image = "http://magalu.com/product/image/1", Price = 10.2M },
                new Product { Id = 2, Title = "Beer (450ml)", Brand = "Budweiser", Image = "http://magalu.com/product/image/2", Price = 3.4M },
                new Product { Id = 3, Title = "LED TV 59 Inch", Brand = "LG", Image = "http://magalu.com/product/image/3", Price = 2890.56M },
                new Product { Id = 4, Title = "Coffee Machine", Brand = "Drive Cofee Inc", Image = "http://magalu.com/product/image/4", Price = 200.5M },
                new Product { Id = 5, Title = "Coffee", Brand = "Dolce Gusto", Image = "http://magalu.com/product/image/5", Price = 200.5M },
                new Product { Id = 6, Title = "Air Frier", Brand = "Panasonic", Image = "http://magalu.com/product/image/6", Price = 200.5M }
            };

        public static Customer[] Customers = new []
            {
                new Customer { Id = 1, Name = "John Doe", Email = "john.doe367@someprovider.com" },
                new Customer { Id = 2, Name = "Mary Dias", Email = "mari-dias@myemail.com" },
                new Customer { Id = 3, Name = "Leo Mango", Email = "leo.mango@another-provider.com" },
                new Customer { Id = 4, Name = "Mary Cell", Email = "mary.cell@new-isp.com" },
                new Customer { Id = 5, Name = "Squall Leonheart", Email = "squall_leonheart@square-enix.com" },
                new Customer { Id = 6, Name = "Alice Farias", Email = "alice.farias@older.isp.com" }
            };

        public static ProductReview[] ProductReviews = new []
            {
                new ProductReview { CustomerId = Customers[0].Id, ProductId = Products[0].Id, Score = 4.1f },
                new ProductReview { CustomerId = Customers[0].Id, ProductId = Products[1].Id, Score = 3.0f },
                new ProductReview { CustomerId = Customers[0].Id, ProductId = Products[2].Id, Score = 5.0f, Comments = "Nice image quality, durable and easy to operate." },
                new ProductReview { CustomerId = Customers[0].Id, ProductId = Products[3].Id, Score = 2.9f },
                new ProductReview { CustomerId = Customers[0].Id, ProductId = Products[4].Id, Score = 4.0f },
                new ProductReview { CustomerId = Customers[0].Id, ProductId = Products[5].Id, Score = 1.7f },
                new ProductReview { CustomerId = Customers[1].Id, ProductId = Products[0].Id, Score = 3.5f },
                new ProductReview { CustomerId = Customers[1].Id, ProductId = Products[1].Id, Score = 4.6f },
                new ProductReview { CustomerId = Customers[2].Id, ProductId = Products[2].Id, Score = 4.8f },
                new ProductReview { CustomerId = Customers[1].Id, ProductId = Products[3].Id, Score = 3.1f },
                new ProductReview { CustomerId = Customers[4].Id, ProductId = Products[1].Id, Score = 5.0f },
                new ProductReview { CustomerId = Customers[5].Id, ProductId = Products[3].Id, Score = 4.0f }
            };

        public static FavoriteProduct[] FavoriteProducts = new []
            {
                new FavoriteProduct { CustomerId = Customers[0].Id, ProductId = Products[2].Id },
                new FavoriteProduct { CustomerId = Customers[2].Id, ProductId = Products[2].Id },
                new FavoriteProduct { CustomerId = Customers[4].Id, ProductId = Products[1].Id },
                new FavoriteProduct { CustomerId = Customers[5].Id, ProductId = Products[3].Id }
            };

        public const string AdminUsername = "admin";

        public const string AdminPassword = "adminpw";

        public const string UserUsername = "user";

        public const string UserPassword = "userpw";

        public const string CustomerUsername = "john";

        public const string CustomerPassword = "johnpw";
    }
}
