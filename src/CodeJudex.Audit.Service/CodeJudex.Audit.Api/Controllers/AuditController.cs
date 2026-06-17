using CodeJudex.Audit.Application.DTOs.Requests;
using CodeJudex.Audit.Application.DTOs.Responses;
using CodeJudex.Audit.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeJudex.Audit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditController(IAuditService auditService) : ControllerBase
{
    /// <summary>
    /// Accepts source code to perform an automated code audit.
    /// </summary>
    /// <param name="request">The DTO containing the source code and programming language.</param>
    /// <param name="ct">The cancellation token to abort the request.</param>
    /// <returns>The analysis results along with the code quality score.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AuditResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuditResponseDto>> AuditCode([FromBody] AuditRequestDto request, CancellationToken ct)
    {
        var result = await auditService.AuditCodeAsync(request, ct);

        return Ok(result);
    }
}