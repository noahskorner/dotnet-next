namespace Api.Services.PasswordService
{
    public interface IPasswordService
    {
        string Hash(string password);

        bool Verify(string password, string actualPassword);
    }

    public class InvalidHashedPasswordFormatException : Exception { }

    public class InvalidPasswordException : Exception { }
}
