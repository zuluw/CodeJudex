using FluentValidation;

namespace CodeJudex.Identity.Application.Features.Auth.Login;

/// <summary>
/// Defines validation rules for the <see cref="LoginCommand"/>.
/// </summary>
public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}