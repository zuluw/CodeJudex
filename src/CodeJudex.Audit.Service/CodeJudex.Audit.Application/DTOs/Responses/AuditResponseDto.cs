namespace CodeJudex.Audit.Application.DTOs.Responses;

/// <summary>
/// DTO for the final audit result in the response
/// </summary>
public record AuditResponseDto(
    int QualityScore,
    List<AuditIssueResponseDto> Issues,
    DateTimeOffset AnalyzedAt
);