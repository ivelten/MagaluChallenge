using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Application.Models.User
{
    public class AuthenticateUserModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Username can not exceed 50 characters length.")]
        public string Username { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Password can not exceed 100 characters length.")]
        public string Password { get; set; }
    }
}
