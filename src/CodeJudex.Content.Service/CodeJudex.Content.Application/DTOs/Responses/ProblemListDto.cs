using CodeJudex.Content.Domain.Enums;

namespace CodeJudex.Content.Application.DTOs.Responses;

/// <summary>
/// Represents a simplified challenge summary for catalog listings.
/// </summary>
public record ProblemListDto(
    Guid Id,
    string Title,
    string Slug,
    Difficulty Difficulty
);