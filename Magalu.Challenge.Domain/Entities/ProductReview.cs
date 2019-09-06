using System;

namespace Magalu.Challenge.Domain.Entities
{
    public class ProductReview
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public float Score { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDateTime { get; set; } = DateTime.Now;
    }
}
