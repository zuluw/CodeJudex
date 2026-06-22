using CodeJudex.Identity.Application.Common.Interfaces;
using CodeJudex.Identity.Application.Common.Mappings;
using CodeJudex.Identity.Application.DTOs.Responses;
using CodeJudex.Identity.Domain.Exceptions;
using CodeJudex.Identity.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CodeJudex.Identity.Application.Features.Auth.Register;

/// <summary>
/// Handles the user registration process for the <see cref="RegisterCommand"/>.
/// </summary>
public class RegisterHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ITokenService tokenService,
    AuthMapper mapper,
    ILogger<RegisterHandler> logger) : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    /// <inheritdoc />
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            logger.LogWarning("Registration failed: Email {Email} is already in use.", request.Email);
            throw new BadRequestException("User with this email already exists.");
        }

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("User creation failed for {Email}: {Errors}", request.Email, errors);
            throw new BadRequestException(errors);
        }

        var roleName = request.Role.ToString();
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new Role(roleName));
        }

        await userManager.AddToRoleAsync(user, roleName);

        var roles = new List<string> { roleName };
        var (accessToken, refreshToken, expiresAt) = tokenService.GenerateTokens(user, roles);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiresAt.AddDays(7);
        await userManager.UpdateAsync(user);

        logger.LogInformation("New user {Email} registered successfully with role {Role}.", user.Email, roleName);

        return mapper.MapToResponse(user, accessToken, refreshToken, expiresAt);
    }
}