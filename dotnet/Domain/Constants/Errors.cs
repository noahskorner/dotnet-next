namespace Domain.Constants
{
    public static class Errors
    {
        public const string UNKNOWN = "An unkown issue has occurred. Please try again.";
        public const string CREATE_USER_AREADY_EXISTS = "A user with that email already exists.";
        public const string CREATE_USER_NOT_FOUND = "This user does not exist.";
        public const string VERIFY_EMAIL_INVALID_TOKEN = "Your token was invalid.";
        public const string LOGIN_USER_INVALID_EMAIL_OR_PASSWORD = "Incorrect email or password. Please try again.";
    }
}
