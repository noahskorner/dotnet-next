using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Api.Utilities
{
    public interface IPasswordManager
    {
        string Hash(string password);

        bool Verify(string password, string actualPassword);
    }

    public class InvalidHashedPasswordFormatException : Exception { }

    public class InvalidPasswordException : Exception { }
}
