using CodeJudex.Audit.Domain.Enums;

namespace CodeJudex.Audit.Domain.Models;

public record AuditIssue(
    string RuleId,
    string Message,
    Severity Severity,
    int LineNumber
);