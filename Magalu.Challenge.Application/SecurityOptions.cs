namespace Magalu.Challenge.Application
{
    public class SecurityOptions
    {
        public string JwtSecret { get; set; }

        public int TokenExpirationTimeInMinutes { get; set; }
    }
}
