namespace Magalu.Challenge.Web.Api
{
    public class SecurityOptions
    {
        public string JwtSecret { get; set; }

        public int TokenExpirationTimeInMinutes { get; set; }
    }
}
