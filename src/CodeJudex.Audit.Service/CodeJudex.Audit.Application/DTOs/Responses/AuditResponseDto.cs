namespace CodeJudex.Audit.Application.DTOs.Responses;

/// <summary>
/// Represents the final audit report response.
/// </summary>
public record AuditResponseDto(
    int QualityScore,
    List<AuditIssueResponseDto> Issues,
    DateTimeOffset AnalyzedAt
);