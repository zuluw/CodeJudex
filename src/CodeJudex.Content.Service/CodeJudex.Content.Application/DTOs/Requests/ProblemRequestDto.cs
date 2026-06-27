using CodeJudex.Content.Domain.Enums;

namespace CodeJudex.Content.Application.DTOs.Requests;

/// <summary>
/// Represents the data required to create a new programming challenge.
/// </summary>
public record ProblemRequestDto(
    string Title,
    string Description,
    Difficulty Difficulty,
    int MemoryLimitMb,
    int CpuLimitMs,
    IEnumerable<TestCaseRequestDto> TestCases
);