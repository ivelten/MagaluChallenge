using System;

namespace Magalu.Challenge.Domain.Entities
{
    public class FavoriteProduct
    {
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
