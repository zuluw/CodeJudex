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
    /// Analyzes source code and provides an automated audit report.
    /// </summary>
    /// <param name="request">The data transfer object containing source code and language.</param>
    /// <param name="ct">The cancellation token used to abort the request.</param>
    /// <returns>An audit report including detected issues and a quality score.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AuditResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuditResponseDto>> AuditCode([FromBody] AuditRequestDto request, CancellationToken ct)
    {
        var result = await auditService.AuditCodeAsync(request, ct);
        return Ok(result);
    }
}