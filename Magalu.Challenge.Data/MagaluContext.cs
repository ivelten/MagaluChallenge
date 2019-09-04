using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Magalu.Challenge.Data
{
    public class MagaluContext : DbContext
    {
        public MagaluContext(DbContextOptions options)
            : base(options)
        {
            SetupDatabase();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(customer =>
            {
                customer.HasKey(c => c.Id);
                customer.HasAlternateKey(c => c.Email);
                customer.Property(c => c.Name).IsRequired().HasMaxLength(100);
                customer.Property(c => c.Email).IsRequired().HasMaxLength(254);
                customer.ToTable("customer");
            });

            builder.Entity<Product>(product =>
            {
                product.HasKey(p => p.Id);
                product.Property(p => p.Price).IsRequired().HasColumnType("DECIMAL(13,2)");
                product.Property(p => p.Brand).IsRequired().HasMaxLength(50);
                product.Property(p => p.Image).IsRequired().HasMaxLength(2048);
                product.Property(p => p.Title).IsRequired().HasMaxLength(50);
                product.ToTable("product");
            });

            builder.Entity<ProductReview>(review =>
            {
                review.HasKey(r => new { r.ProductId, r.CustomerId });
                review.HasOne(r => r.Customer).WithMany(c => c.ProductReviews).HasForeignKey(r => r.CustomerId);
                review.HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId);
                review.Property(r => r.Score).IsRequired().HasColumnType("FLOAT(2,1)");
                review.Property(r => r.Comments).HasMaxLength(2000);
                review.Property(r => r.ReviewDateTime).HasDefaultValue(DateTime.Now);
                review.ToTable("product_review");
            });

            builder.Entity<FavoriteProduct>(favorite =>
            {
                favorite.HasKey(f => new { f.ProductId, f.CustomerId });
                favorite.HasOne(f => f.Product).WithMany(p => p.FavoriteCustomers).HasForeignKey(f => f.ProductId);
                favorite.HasOne(f => f.Customer).WithMany(c => c.FavoriteProducts).HasForeignKey(f => f.CustomerId);
                favorite.ToTable("favorite_product");
            });
        }

        private void SetupDatabase()
        {
            Database.EnsureCreated();

            if (!Products.Any())
            {
                var products = new Product[] 
                {
                    new Product { Title = "Cerveja 500ML", Brand = "Heineken", Image = "http://magalu.com/product/image/1", Price = 10.2M },
                    new Product { Title = "Água Mineral 500ML", Brand = "Campinho", Image = "http://magalu.com/product/image/2", Price = 3.4M },
                    new Product { Title = "Televisão 59 Polegadas", Brand = "LG", Image = "http://magalu.com/product/image/3", Price = 2890.56M },
                    new Product { Title = "Cafeteira", Brand = "Arno", Image = "http://magalu.com/product/image/4", Price = 200.5M },
                    new Product { Title = "Cafeteira", Brand = "Walita", Image = "http://magalu.com/product/image/5", Price = 200.5M },
                    new Product { Title = "Fritadeira Elétrica", Brand = "Walita", Image = "http://magalu.com/product/image/6", Price = 200.5M },
                };

                Products.AddRange(products);
            }

            SaveChanges();
        }
    }
}
