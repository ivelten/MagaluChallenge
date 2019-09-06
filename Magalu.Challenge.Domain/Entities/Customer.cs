using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Magalu.Challenge.Domain.Entities
{
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
}
