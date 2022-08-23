using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Services.Services
{
    public interface IPasswordService
    {
        string Hash(string password);

        bool Verify(string password, string actualPassword);
    }

    public class PasswordService : IPasswordService
    {
        public string Hash(string password)
        {
            if (password.Contains(".")) throw new InvalidPasswordException();

            var salt = RandomNumberGenerator.GetBytes(128 / 8);

            var hashedPassword = HashString(password, salt);
            var base64Salt = Convert.ToBase64String(salt);

            return $"{hashedPassword}.{base64Salt}";
        }

        public bool Verify(string password, string hashedPassword)
        {
            try
            {
                var splitPasswords = hashedPassword.Split(".", 2);
                if (splitPasswords.Length != 2) throw new InvalidHashedPasswordFormatException();

                var base64Salt = splitPasswords[1];
                var salt = Convert.FromBase64String(base64Salt);
                var hash = HashString(password, salt);

                var checkPassword = $"{hash}.{base64Salt}";

                return hashedPassword == checkPassword;
            }
            catch
            {
                return false;
            }
        }

        private string HashString(string str, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }

    public class InvalidHashedPasswordFormatException : Exception { }

    public class InvalidPasswordException : Exception { }
}
