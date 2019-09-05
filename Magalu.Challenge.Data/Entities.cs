using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Magalu.Challenge.Data
{
    public class Product
    {
        public long Id { get; set; }

        public decimal Price { get; set; }

        public string Brand { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public virtual ICollection<ProductReview> Reviews { get; set; } = new Collection<ProductReview>();

        public virtual ICollection<FavoriteProduct> FavoriteCustomers { get; set; } = new Collection<FavoriteProduct>();
    }

    public class Customer
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; } = new Collection<ProductReview>();

        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new Collection<FavoriteProduct>();

        public virtual User User { get; set; }
    }

    public class ProductReview
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public float Score { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDateTime { get; set; }
    }

    public class FavoriteProduct
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }

    public class User
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public long? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
