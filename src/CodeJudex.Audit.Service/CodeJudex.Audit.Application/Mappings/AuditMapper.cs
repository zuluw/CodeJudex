using CodeJudex.Audit.Application.DTOs.Responses;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Enums;
using Riok.Mapperly.Abstractions;

namespace CodeJudex.Audit.Application.Mappings;

/// <summary>
/// Mapper for converting audit domain models into data transfer objects
/// </summary>
[Mapper]
public partial class AuditMapper
{
    /// <summary>
    /// Converts the final audit result from the domain model into an API response
    /// </summary>
    public partial AuditResponseDto MapToResponse(AuditResult result);

    /// <summary>
    /// Helper mapping for individual audit issues
    /// </summary>
    /// <remarks>
    /// Mapperly automatically uses this method when processing a list in MapToResponse
    /// </remarks>
    private partial AuditIssueResponseDto MapIssue(AuditIssue issue);

    /// <summary>
    /// Custom mapping for converting the Severity enum to a string for the DTO
    /// </summary>
    private string MapSeverityToString(Severity severity)
    {
        return severity.ToString();
    }
}