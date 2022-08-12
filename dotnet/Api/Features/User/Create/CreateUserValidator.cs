﻿using Api.Data;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Api.Features.User.Create
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Must provide a valid ");

            RuleFor(x => x.Password)
                .MinimumLength(DataConfiguration.MIN_PASSWORD_LENGTH)
                .MaximumLength(DataConfiguration.SHORT_STRING_LENGTH)
                .WithMessage($"{nameof(CreateUserRequest.Password)} must be between {DataConfiguration.MIN_PASSWORD_LENGTH}-{DataConfiguration.SHORT_STRING_LENGTH} characters.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasUpperChar = new Regex(@"[A-Z]+");

                    return hasUpperChar.IsMatch(password);
                })
                .WithMessage($"{nameof(CreateUserRequest.Password)} must contain an uppercase letter.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasNumber = new Regex(@"[0-9]+");

                    return hasNumber.IsMatch(password);
                })
                .WithMessage($"{nameof(CreateUserRequest.Password)} must contain a number.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|/?,-]");

                    return hasSymbols.IsMatch(password);
                })
                .WithMessage($"{nameof(CreateUserRequest.Password)} must contain a symbol.");
            RuleFor(x => x.Password)
                .Must(password =>
                {
                    var validCharacters = new Regex(@"\A[!@#$%^&*()_+=\[{\]};:<>|/?,A-Za-z0-9]\Z");
                    var isMatch = validCharacters.IsMatch(password);

                    return validCharacters.IsMatch(password);
                })
                .WithMessage($"{nameof(CreateUserRequest.Password)} must only contain numbers, letters, and symbols (!@#$%^&*()_+=\\[{{]}};:<>|/?,)");
        }
    }
}
