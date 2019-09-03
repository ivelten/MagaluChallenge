using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Data
{
    public class Product
    {
        public long Id { get; set; }

        public decimal Price { get; set; }

        public string Brand { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public virtual IList<CustomerProductReview> CustomerReviews { get; set; }

        public virtual IList<CustomerFavoriteProduct> FavoriteCustomers { get; set; }
    }

    public class Customer
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [MaxLength(50, ErrorMessage = "Customer name should not have more than 100 characters length.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer e-mail is required.")]
        [MaxLength(254, ErrorMessage = "Customer e-mail should not have more than 254 characters length.")]
        public string Email { get; set; }

        public virtual IList<CustomerProductReview> ProductReviews { get; set; }

        public virtual IList<CustomerFavoriteProduct> FavoriteProducts { get; set; }
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
