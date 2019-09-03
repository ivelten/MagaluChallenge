using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Web.Api.Models.Customer
{
    public class PostCustomerModel
    {
        [Required(ErrorMessage = "Customer name is required.")]
        // Most browsers and search engines limit URL maximum size to 2048 characters
        [MaxLength(50, ErrorMessage = "Customer name should not have more than 100 characters length.")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Customer e-mail is required.")]
        // In accordance with RFC 2821, e-mail addresses should not have more than 254 characters
        [MaxLength(254, ErrorMessage = "Customer e-mail should not have more than 254 characters length.")]
        public string Email { get; set; }
    }
}
