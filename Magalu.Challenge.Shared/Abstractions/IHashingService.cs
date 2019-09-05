namespace Magalu.Challenge.Shared.Abstractions
{
    public interface IHashingService
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);
    }
}
