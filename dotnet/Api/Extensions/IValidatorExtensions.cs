using FluentValidation;

namespace Api.Extensions
{
    public static class IValidatorExtensions
    {
        public static async Task ValidateAsyncOrThrow<TRequest>(this IValidator validator, TRequest request) {
            var context = new ValidationContext<TRequest>(request);
            var validationResult = await validator.ValidateAsync(context);
            var failures = validationResult.Errors
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }
    }
}
