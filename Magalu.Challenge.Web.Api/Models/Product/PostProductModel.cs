using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Web.Api.Models.Product
{
    public class PostProductModel
    {
        [MaxPrecision(2, ErrorMessage = "Product price decimal places should have between 0 to 2 digits.")]
        [Range(0.01d, 99999999999.99d, ErrorMessage = "Product price must be between 0.01 and 99999999999.99.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product brand is required.")]
        [MaxLength(50, ErrorMessage = "Product brand should not have more than 50 characters length.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Product image URL is required.")]
        [MaxLength(2048, ErrorMessage = "Product image URL should not have more than 2048 characters length.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Product title is required.")]
        [MaxLength(50, ErrorMessage = "Product title should not have more than 50 characters length.")]
        public string Title { get; set; }
    }
}
