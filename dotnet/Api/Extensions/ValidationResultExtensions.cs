using Api.Enumerations;
using Api.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Api.Extensions
{
    public static class ValidationResultExtensions
    {
        public static async Task<Result<T>> ToResult<T>(this Task<ValidationResult> validationResultTask)
        {
            var validationResult = await validationResultTask;
            var errors = validationResult.Errors
                .Where(x => x != null)
                .Select(x => new Error(ErrorType.Validation, x.ErrorMessage, field: x.PropertyName));

            return new Result<T>(errors: errors);
        }
    }
}
