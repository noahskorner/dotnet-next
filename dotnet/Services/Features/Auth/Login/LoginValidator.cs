using FluentValidation;

namespace Services.Features.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage($"Must provide a valid {nameof(LoginCommand.Email)}");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage($"Must provide a {nameof(LoginCommand.Password)}");
        }
    }
}
