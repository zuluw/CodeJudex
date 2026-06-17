namespace CodeJudex.Audit.Domain.Models;

public record AuditResult(
    int QualityScore,
    IReadOnlyCollection<AuditIssue> Issues,
    DateTimeOffset AnalyzedAt
);