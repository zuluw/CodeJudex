using CodeJudex.Audit.Application.DTOs.Requests;
using CodeJudex.Audit.Application.DTOs.Responses;

namespace CodeJudex.Audit.Application.Interfaces;

/// <summary>
/// Interface for the main service responsible for conducting code audits
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Conducts a full audit of the submitted code
    /// </summary>
    /// <param name="request">DTO with the source code and language</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>DTO with the analysis results and quality score</returns>
    Task<AuditResponseDto> AuditCodeAsync(AuditRequestDto request, CancellationToken ct);
}