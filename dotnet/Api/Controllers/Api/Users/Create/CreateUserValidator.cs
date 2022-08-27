using Api.Constants;
using Data.Configuration;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace Api.Controllers.Api.Users.Create
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly IStringLocalizer _localizer;

        public CreateUserValidator(IStringLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(_localizer.GetString(ValidationErrors.CREATE_USER_EMAIL_NOT_VALID));

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(DataConfiguration.MIN_PASSWORD_LENGTH)
                .MaximumLength(DataConfiguration.SHORT_STRING_LENGTH)
                .WithMessage(_localizer.GetString(
                    ValidationErrors.CREATE_USER_PASSWORD_LENGTH,
                    DataConfiguration.MIN_PASSWORD_LENGTH,
                    DataConfiguration.SHORT_STRING_LENGTH));
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasUpperChar = new Regex(@"[A-Z]+");

                    return hasUpperChar.IsMatch(password);
                })
                .WithMessage(_localizer.GetString(ValidationErrors.CREATE_USER_PASSWORD_MUST_CONTAIN_UPPERCASE));
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasNumber = new Regex(@"[0-9]+");

                    return hasNumber.IsMatch(password);
                })
                .WithMessage(_localizer.GetString(ValidationErrors.CREATE_USER_PASSWORD_MUST_CONTAIN_NUMBER));
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|/?,-]");

                    return hasSymbols.IsMatch(password);
                })
                .WithMessage(_localizer.GetString(ValidationErrors.CREATE_USER_PASSWORD_MUST_CONTAIN_SYMBOL));
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var validCharacters = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|/?,A-Za-z0-9]");
                    var invalidCharacters = validCharacters.Replace(password, "");

                    return !invalidCharacters.Any();
                })
                .WithMessage(_localizer.GetString(ValidationErrors.CREATE_USER_PASSWORD_NOT_VALID));
        }
    }
}
