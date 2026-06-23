using CodeJudex.Identity.Application.DTOs.Responses;
using MediatR;

namespace CodeJudex.Identity.Application.Features.Auth.Login;

/// <summary>
/// Represents a command to authenticate a user.
/// </summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's password.</param>
public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;