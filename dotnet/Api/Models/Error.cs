using Domain.Enumerations;

namespace Api.Models
{
    public class Error
    {
        public ErrorType Type { get; }
        public string Message { get; }
        public string? Key { get; }
        public string? Field { get; }

        public Error(
            ErrorType type,
            string message,
            string? key = null,
            string? field = null)
        {
            Type = type;
            Message = message;
            Key = key;
            Field = field;
        }
    }
}
