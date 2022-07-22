namespace Api.Services.PasswordManager
{
    public interface IPasswordManager
    {
        string Hash(string password);

        bool Verify(string password, string actualPassword);
    }

    public class InvalidHashedPasswordFormatException : Exception { }

    public class InvalidPasswordException : Exception { }
}
