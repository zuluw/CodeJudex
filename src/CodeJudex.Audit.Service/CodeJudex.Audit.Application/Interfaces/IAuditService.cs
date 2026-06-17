using CodeJudex.Audit.Application.DTOs.Requests;
using CodeJudex.Audit.Application.DTOs.Responses;

namespace CodeJudex.Audit.Application.Interfaces;

/// <summary>
/// Defines the service for coordinating code audits.
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Performs a comprehensive code audit based on the provided request.
    /// </summary>
    Task<AuditResponseDto> AuditCodeAsync(AuditRequestDto request, CancellationToken ct);
}