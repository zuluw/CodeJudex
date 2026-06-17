namespace CodeJudex.Audit.Application.DTOs.Responses;

/// <summary>
/// Represents a single code violation in the audit response.
/// </summary>
public record AuditIssueResponseDto(
    string RuleId,
    string Message,
    string Severity,
    int LineNumber
);