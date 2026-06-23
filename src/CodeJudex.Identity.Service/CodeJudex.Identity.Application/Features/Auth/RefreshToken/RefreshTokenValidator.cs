using FluentValidation;

namespace CodeJudex.Identity.Application.Features.Auth.RefreshToken;

/// <summary>
/// Defines validation rules for the <see cref="RefreshTokenCommand"/>.
/// </summary>
public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}