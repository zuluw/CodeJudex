using CodeJudex.Identity.Application.DTOs.Responses;
using CodeJudex.Identity.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace CodeJudex.Identity.Application.Common.Mappings;

/// <summary>
/// Provides mapping functionality for authentication data.
/// </summary>
[Mapper]
public partial class AuthMapper
{
    /// <summary>
    /// Maps a <see cref="User"/> and token data to an <see cref="AuthResponseDto"/>.
    /// </summary>
    public partial AuthResponseDto MapToResponse(User user, string accessToken, string refreshToken, DateTime expiresAt);
}