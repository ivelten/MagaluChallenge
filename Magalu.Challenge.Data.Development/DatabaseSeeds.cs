using Magalu.Challenge.Domain.Entities;
using System;
using System.Linq;

namespace Magalu.Challenge.Data.Development
{
    public static class DatabaseSeeds
    {
        public static Product[] Products = new[]
            {
                new Product { Id = new Guid("f03b7c08-0e9f-42a8-8d93-13b8428a8eb0"), Title = "Beer (500ml)", Brand = "Heineken", Image = "http://magalu.com/product/image/1", Price = 10.2M },
                new Product { Id = new Guid("2475727e-1daa-4581-a22c-6fae8df2baa1"), Title = "Beer (450ml)", Brand = "Budweiser", Image = "http://magalu.com/product/image/2", Price = 3.4M },
                new Product { Id = new Guid("b9e8f4ba-09ce-479a-8754-ca39d9809a60"), Title = "LED TV 59 Inch", Brand = "LG", Image = "http://magalu.com/product/image/3", Price = 2890.56M },
                new Product { Id = new Guid("e8696273-ebbc-4542-bdbd-4a070d20d524"), Title = "Coffee Machine", Brand = "Drive Cofee Inc", Image = "http://magalu.com/product/image/4", Price = 200.5M },
                new Product { Id = new Guid("33e57519-255f-4144-b514-3e3f08977543"), Title = "Coffee", Brand = "Dolce Gusto", Image = "http://magalu.com/product/image/5", Price = 200.5M },
                new Product { Id = new Guid("d7d334e1-c596-451f-8af3-40a40109e563"), Title = "Air Frier", Brand = "Panasonic", Image = "http://magalu.com/product/image/6", Price = 200.5M }
            }.OrderBy(p => p.Id).ToArray();

        public static Customer[] Customers = new []
            {
                new Customer { Id = new Guid("0d208fc9-c5ee-4638-8ff7-fdd3ac48d591"), Name = "John Doe", Email = "john.doe367@someprovider.com" },
                new Customer { Id = new Guid("3082ee64-8974-4bea-8ea5-80446876d0dd"), Name = "Mary Dias", Email = "mari-dias@myemail.com" },
                new Customer { Id = new Guid("19d9130e-b7f3-4470-b95b-2cc22efcb756"), Name = "Leo Mango", Email = "leo.mango@another-provider.com" },
                new Customer { Id = new Guid("68dd3034-460b-48bb-b196-f9713ccd2cb2"), Name = "Mary Cell", Email = "mary.cell@new-isp.com" },
                new Customer { Id = new Guid("23d09075-58f5-4ccc-aae4-d021b2f3939a"), Name = "Squall Leonheart", Email = "squall_leonheart@square-enix.com" },
                new Customer { Id = new Guid("20b0afa4-2e5e-45f4-9e20-2988d89d2749"), Name = "Alice Farias", Email = "alice.farias@older.isp.com" }
            }.OrderBy(c => c.Id).ToArray();

        public const string AdminUsername = "admin";

        public const string AdminPassword = "adminpw";

        public const string UserUsername = "user";

        public const string UserPassword = "userpw";

        public const string CustomerUsername = "john";

        public const string CustomerPassword = "johnpw";
    }
}
