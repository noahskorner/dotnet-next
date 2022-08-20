namespace Domain.Constants
{
    public static class Errors
    {
        public const string UNKNOWN = "An unkown issue has occurred. Please try again.";
        public const string USER_ALREADY_EXISTS = "A user with that email already exists.";
        public const string USER_NOT_FOUND = "This user does not exist.";
        public const string INVALID_EMAIL_VERIFICATION_TOKEN = "Your token was invalid.";
    }
}
