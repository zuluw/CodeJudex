using CodeJudex.Identity.Application.DTOs.Responses;
using CodeJudex.Identity.Domain.Enums;
using MediatR;

namespace CodeJudex.Identity.Application.Features.Auth.Register;

/// <summary>
/// Represents a command to register a new user in the system.
/// </summary>
public record RegisterCommand(string Email, string Password, string FullName, UserRoles Role = UserRoles.Student) : IRequest<AuthResponseDto>;