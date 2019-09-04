using Magalu.Challenge.Shared.Abstractions;

namespace Magalu.Challenge.Security
{
    public class BCryptHashingService : IHashingService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
