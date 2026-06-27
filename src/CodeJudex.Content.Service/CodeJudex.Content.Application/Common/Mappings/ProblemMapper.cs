using CodeJudex.Content.Application.DTOs.Responses;
using CodeJudex.Content.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace CodeJudex.Content.Application.Common.Mappings;

/// <summary>
/// Provides mapping functionality between <see cref="Problem"/> domain models and DTOs.
/// </summary>
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class ProblemMapper
{
    public partial ProblemListDto MapToListDto(Problem problem);
    public ProblemResponseDto MapToResponseDto(Problem problem)
    {
        return new ProblemResponseDto(
            problem.Id,
            problem.Title,
            problem.Slug,
            problem.Description,
            problem.Difficulty,
            problem.MemoryLimitMb,
            problem.CpuLimitMs,
            problem.TestCases
                .Where(tc => !tc.IsHidden)
                .Select(MapTestCase)
        );
    }
    private partial TestCaseResponseDto MapTestCase(TestCase testCase);
}