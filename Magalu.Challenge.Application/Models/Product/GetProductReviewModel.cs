using System;

namespace Magalu.Challenge.Application.Models.Product
{
    public class GetProductReviewModel
    {
        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }

        public float Score { get; set; }

        public string Comments { get; set; }
    }
}
