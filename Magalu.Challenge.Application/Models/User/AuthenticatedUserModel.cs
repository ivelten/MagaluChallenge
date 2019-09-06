namespace Magalu.Challenge.Application.Models.User
{
    public class AuthenticatedUserModel
    {
        public string Username { get; set; }

        public string JwtToken { get; set; }
    }
}
