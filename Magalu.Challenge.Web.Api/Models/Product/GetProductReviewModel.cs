namespace Magalu.Challenge.Web.Api.Models.Product
{
    public class GetProductReviewModel
    {
        public long CustomerId { get; set; }

        public long ProductId { get; set; }

        public float Score { get; set; }

        public string Comments { get; set; }
    }
}
