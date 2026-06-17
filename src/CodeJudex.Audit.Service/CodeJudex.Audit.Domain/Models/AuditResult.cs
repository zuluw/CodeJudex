namespace CodeJudex.Audit.Domain.Models;

/// <summary>
/// Final report of the code audit
/// </summary>
public record AuditResult(
    int QualityScore,
    IReadOnlyCollection<AuditIssue> Issues,
    DateTimeOffset AnalyzedAt
);