namespace Magalu.Challenge.Web.Api.Models.User
{
    public class AuthenticatedUserModel
    {
        public string Username { get; set; }

        public string JwtToken { get; set; }
    }
}
