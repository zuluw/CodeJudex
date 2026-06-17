using CodeJudex.Audit.Domain.Enums;

namespace CodeJudex.Audit.Domain.Models;

/// <summary>
/// Description of a specific code violation found
/// </summary>
public record AuditIssue(
    string RuleId,
    string Message,
    Severity Severity,
    int LineNumber
);