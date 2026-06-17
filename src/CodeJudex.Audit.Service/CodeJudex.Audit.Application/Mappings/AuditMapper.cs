using CodeJudex.Audit.Application.DTOs.Responses;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Enums;
using Riok.Mapperly.Abstractions;

namespace CodeJudex.Audit.Application.Mappings;

/// <summary>
/// Provides mapping functionality between domain models and DTOs.
/// </summary>
[Mapper]
public partial class AuditMapper
{
    public partial AuditResponseDto MapToResponse(AuditResult result);
    private partial AuditIssueResponseDto MapIssue(AuditIssue issue);
    private string MapSeverityToString(Severity severity)
    {
        return severity.ToString();
    }
}