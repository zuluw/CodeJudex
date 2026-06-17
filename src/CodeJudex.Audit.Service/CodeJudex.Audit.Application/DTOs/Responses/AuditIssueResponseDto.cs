namespace CodeJudex.Audit.Application.DTOs.Responses;

/// <summary>
/// DTO for describing a specific violation in the response
/// </summary>
public record AuditIssueResponseDto(
    string RuleId,
    string Message,
    string Severity,
    int LineNumber
);