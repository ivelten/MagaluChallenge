using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Web.Api.Models.Customer
{
    public class SendFavoriteProductModel
    {
        [Required]
        public long ProductId { get; set; }
    }
}
