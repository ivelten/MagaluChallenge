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

        public virtual ICollection<CustomerProductReview> CustomerReviews { get; set; } = new Collection<CustomerProductReview>();

        public virtual ICollection<CustomerFavoriteProduct> FavoriteCustomers { get; set; } = new Collection<CustomerFavoriteProduct>();
    }

    public class Customer
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public virtual ICollection<CustomerProductReview> ProductReviews { get; set; } = new Collection<CustomerProductReview>();

        public virtual ICollection<CustomerFavoriteProduct> FavoriteProducts { get; set; } = new Collection<CustomerFavoriteProduct>();
    }

    public class CustomerProductReview
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public float Score { get; set; }

        public string Comments { get; set; }
    }

    public class CustomerFavoriteProduct
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
