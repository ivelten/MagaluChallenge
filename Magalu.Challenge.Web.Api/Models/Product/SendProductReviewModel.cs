using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Web.Api.Models.Product
{
    public class SendProductReviewModel
    {
        public long CustomerId { get; set; }

        [Range(0, 5, ErrorMessage = "Product review score must be a number between 0 and 5.")]
        [MaxPrecision(1, ErrorMessage = "Product review score decimal places must have 1 digit maximum.")]
        public float Score { get; set; }

        [MaxLength(2000, ErrorMessage = "Product review comments can not exceed 2.000 characters.")]
        public string Comments { get; set; }
    }
}
