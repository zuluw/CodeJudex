using CodeJudex.Identity.Application.Common.Interfaces;
using CodeJudex.Identity.Application.Common.Mappings;
using CodeJudex.Identity.Application.DTOs.Responses;
using CodeJudex.Identity.Domain.Exceptions;
using CodeJudex.Identity.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CodeJudex.Identity.Application.Features.Auth.RefreshToken;

/// <summary>
/// Handles the token refresh process for the <see cref="RefreshTokenCommand"/>.
/// </summary>
public class RefreshTokenHandler(
    UserManager<User> userManager,
    ITokenService tokenService,
    AuthMapper mapper,
    ILogger<RefreshTokenHandler> logger) : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    /// <inheritdoc />
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var principal = tokenService.GetClaimsFromToken(request.AccessToken);
        var email = principal.Identity?.Name;

        if (string.IsNullOrEmpty(email))
        {
            throw new UnauthorizedException("Invalid access token claims.");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            logger.LogWarning("Invalid refresh token attempt for user: {Email}", email);
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        var roles = await userManager.GetRolesAsync(user);
        var (newAccessToken, newRefreshToken, expiresAt) = tokenService.GenerateTokens(user, roles);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = expiresAt.AddDays(7);
        await userManager.UpdateAsync(user);    

        logger.LogInformation("Tokens refreshed successfully for user: {Email}", email);

        return mapper.MapToResponse(user, newAccessToken, newRefreshToken, expiresAt);
    }
}