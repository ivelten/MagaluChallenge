using System;
using Microsoft.EntityFrameworkCore;

namespace Magalu.Challenge.Data
{
    public class MagaluContext : DbContext
    {
        public MagaluContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(customer =>
            {
                customer.HasKey(c => c.Id);
                customer.HasAlternateKey(c => c.Email);
                customer.Property(c => c.Name).IsRequired().HasMaxLength(100);
                customer.Property(c => c.Email).IsRequired().HasMaxLength(254);
                customer.HasOne(c => c.User).WithOne(u => u.Customer).HasForeignKey<Customer>(c => c.Username);
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

            builder.Entity<User>(user =>
            {
                user.HasKey(u => u.Username);
                user.Property(u => u.Username).HasMaxLength(50);
                user.Property(u => u.PasswordHash).HasMaxLength(1024);
                user.Property(u => u.Role).HasMaxLength(50);
                user.HasOne(u => u.Customer).WithOne(c => c.User).HasForeignKey<User>(u => u.CustomerId);
                user.ToTable("user");
            });
        }
    }
}
