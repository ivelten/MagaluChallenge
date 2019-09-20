using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Magalu.Challenge.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public string Brand { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public virtual ICollection<ProductReview> Reviews { get; set; } = new Collection<ProductReview>();

        public virtual ICollection<FavoriteProduct> FavoriteCustomers { get; set; } = new Collection<FavoriteProduct>();
    }
}
