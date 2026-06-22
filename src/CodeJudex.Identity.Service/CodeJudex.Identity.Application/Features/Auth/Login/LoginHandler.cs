using CodeJudex.Identity.Application.Common.Interfaces;
using CodeJudex.Identity.Application.Common.Mappings;
using CodeJudex.Identity.Application.DTOs.Responses;
using CodeJudex.Identity.Domain.Exceptions;
using CodeJudex.Identity.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CodeJudex.Identity.Application.Features.Auth.Login;

/// <summary>
/// Handles the authentication process for the <see cref="LoginCommand"/>.
/// </summary>
public class LoginHandler(
    UserManager<User> userManager,
    ITokenService tokenService,
    AuthMapper mapper,
    ILogger<LoginHandler> logger) : IRequestHandler<LoginCommand, AuthResponseDto>
{
    /// <inheritdoc />
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            logger.LogWarning("Unauthorized access attempt for email: {Email}", request.Email);
            throw new UnauthorizedException("Invalid email or password.");
        }

        var roles = await userManager.GetRolesAsync(user);

        var (accessToken, refreshToken, expiresAt) = tokenService.GenerateTokens(user, roles);

        // TODO: Save Refresh Token to the database after implementing Infrastructure layer.
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiresAt.AddDays(7);

        await userManager.UpdateAsync(user);

        logger.LogInformation("User {Email} authenticated successfully.", user.Email);

        return mapper.MapToResponse(user, accessToken, refreshToken, expiresAt);
    }
}