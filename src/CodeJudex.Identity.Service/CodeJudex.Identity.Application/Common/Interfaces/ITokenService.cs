using CodeJudex.Identity.Domain.Models;
using System.Security.Claims;

namespace CodeJudex.Identity.Application.Common.Interfaces;

/// <summary>
/// Defines the service for generating security tokens.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates access and refresh tokens for the specified user.
    /// </summary>
    (string AccessToken, string RefreshToken, DateTime ExpiresAt) GenerateTokens(User user, IList<string> roles);

    /// <summary>
    /// Extracts user claims from an expired access token for verification.
    /// </summary>
    ClaimsPrincipal GetClaimsFromToken(string token);

}