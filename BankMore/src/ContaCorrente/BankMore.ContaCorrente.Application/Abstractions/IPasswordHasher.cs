namespace BankMore.Application.Abstractions
{
    public interface IPasswordHasher
    {
        (string hash, string salt) Hash(string plainTextPassword);
    }
}
