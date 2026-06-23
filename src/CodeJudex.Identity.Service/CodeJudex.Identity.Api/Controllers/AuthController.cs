using CodeJudex.Identity.Application.DTOs.Responses;
using CodeJudex.Identity.Application.Features.Auth.Login;
using CodeJudex.Identity.Application.Features.Auth.RefreshToken;
using CodeJudex.Identity.Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeJudex.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and returns security tokens.
    /// </summary>
    /// <param name="command">The login credentials.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing access and refresh tokens.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>
    /// Registers a new user and returns security tokens.
    /// </summary>
    /// <param name="command">The registration data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>An <see cref="AuthResponseDto"/> for the newly created user.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes security tokens using a valid refresh token.
    /// </summary>
    /// <param name="command">The current access and refresh tokens.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A new set of security tokens.</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}