using CodeJudex.Content.Domain.Enums;

namespace CodeJudex.Content.Application.DTOs.Responses;

/// <summary>
/// Represents a detailed challenge view including resource limits and public test cases.
/// </summary>
public record ProblemResponseDto(
    Guid Id,
    string Title,
    string Slug,
    string Description,
    Difficulty Difficulty,
    int MemoryLimitMb,
    int CpuLimitMs,
    IEnumerable<TestCaseResponseDto> TestCases
    );