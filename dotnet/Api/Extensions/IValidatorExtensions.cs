using FluentValidation;

namespace Api.Extensions
{
    public static class IValidatorExtensions
    {
        public static async Task ValidateAsyncOrThrow<T>(this IValidator<T> validator, T request)
        {
            var validationResult = await validator.ValidateAsync(request);
            var failures = validationResult.Errors.Where(x => x != null).ToList();

            if (failures.Any()) throw new ValidationException(failures);
        }
    }
}
