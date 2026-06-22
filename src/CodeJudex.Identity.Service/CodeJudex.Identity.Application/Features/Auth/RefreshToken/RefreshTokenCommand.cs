using CodeJudex.Identity.Application.DTOs.Responses;
using MediatR;

namespace CodeJudex.Identity.Application.Features.Auth.RefreshToken;

/// <summary>
/// Represents a command to refresh access tokens using a valid refresh token.
/// </summary>
public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AuthResponseDto>;