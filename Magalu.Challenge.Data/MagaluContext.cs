using Microsoft.EntityFrameworkCore;

namespace Magalu.Challenge.Data
{
    public class MagaluContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CustomerProductReview> CustomerProductReviews { get; set; }

        public DbSet<CustomerFavoriteProduct> CustomerFavoriteProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(customer =>
            {
                customer.HasKey(c => c.Id);
                customer.Property(c => c.Name).IsRequired().HasMaxLength(100);
                customer.Property(c => c.Email).IsRequired().HasMaxLength(254); // In accordance with RFC 2821
            });

            builder.Entity<Product>(product =>
            {
                product.HasKey(p => p.Id);
                product.Property(p => p.Price).IsRequired().HasColumnType("DECIMAL(13,2)");
                product.Property(p => p.Brand).IsRequired().HasMaxLength(50);
                product.Property(p => p.Image).IsRequired().HasMaxLength(2048); // Most browsers and search engines limit URL maximum size to 2048 characters
                product.Property(p => p.Title).IsRequired().HasMaxLength(50);
            });

            builder.Entity<CustomerProductReview>(review =>
            {
                review.HasKey(r => new { r.ProductId, r.CustomerId });
                review.HasOne(r => r.Customer).WithMany(c => c.ProductReviews).HasForeignKey(r => r.CustomerId);
                review.HasOne(r => r.Product).WithMany(p => p.CustomerReviews).HasForeignKey(r => r.ProductId);
                review.Property(r => r.Score).IsRequired().HasColumnType("FLOAT(2,1)");
                review.Property(r => r.Comments).HasMaxLength(2000);
            });

            builder.Entity<CustomerFavoriteProduct>(favorite =>
            {
                favorite.HasKey(f => new { f.ProductId, f.CustomerId });
                favorite.HasOne(f => f.Product).WithMany(p => p.FavoriteCustomers).HasForeignKey(f => f.ProductId);
                favorite.HasOne(f => f.Customer).WithMany(c => c.FavoriteProducts).HasForeignKey(f => f.CustomerId);
            });
        }
    }
}
