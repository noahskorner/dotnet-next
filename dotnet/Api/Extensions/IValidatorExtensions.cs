using FluentValidation;
using FluentValidation.Results;

namespace Api.Extensions
{
    public static class IValidatorExtensions
    {
        public static async Task<ValidationResult> ValidateAsyncOrThrow<T>(this IValidator<T> validator, T instance)
        {
            var validationResult = await validator.ValidateAsync(instance);

            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            return validationResult;
        }
    }
}
