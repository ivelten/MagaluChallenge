namespace Magalu.Challenge.Domain.Entities
{
    public class FavoriteProduct
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
