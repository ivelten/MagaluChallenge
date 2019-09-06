namespace Magalu.Challenge.ApplicationServices
{
    public interface IHashingService
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);
    }
}
