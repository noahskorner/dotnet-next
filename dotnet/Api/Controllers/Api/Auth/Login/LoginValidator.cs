using Api.Constants;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Api.Controllers.Api.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        private readonly IStringLocalizer _localizer;

        public LoginValidator(IStringLocalizer localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(_localizer.GetString(ValidationErrors.LOGIN_EMAIL_NOT_VALID));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(_localizer.GetString(ValidationErrors.LOGIN_MUST_PROVIDE_PASSWORD));
        }
    }
}
