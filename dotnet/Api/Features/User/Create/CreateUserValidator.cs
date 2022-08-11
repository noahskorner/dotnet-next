using Api.Data;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Api.Features.User.Create
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(DataConfiguration.MIN_PASSWORD_LENGTH)
                .MaximumLength(DataConfiguration.SHORT_STRING_LENGTH)
                .WithMessage(x => $"{nameof(x.Password)} must be between {DataConfiguration.MIN_PASSWORD_LENGTH}-{DataConfiguration.SHORT_STRING_LENGTH} characters.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasUpperChar = new Regex(@"[A-Z]+");

                    return hasUpperChar.IsMatch(password);
                })
                .WithMessage(x => $"{nameof(x.Password)} must contain an uppercase letter.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasNumber = new Regex(@"[0-9]+");

                    return hasNumber.IsMatch(password);
                })
                .WithMessage(x => $"{nameof(x.Password)} must contain a number.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                    return hasSymbols.IsMatch(password);
                })
                .WithMessage(x => $"{nameof(x.Password)} must contain a symbol.");
        }
    }
}
