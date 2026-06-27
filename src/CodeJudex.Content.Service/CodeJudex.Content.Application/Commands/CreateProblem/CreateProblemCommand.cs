using CodeJudex.Content.Application.DTOs.Requests;
using CodeJudex.Content.Domain.Enums;
using MediatR;

namespace CodeJudex.Content.Application.Commands.CreateProblem;

/// <summary>
/// Represents a command to create a new programming problem.
/// </summary>
public record CreateProblemCommand(
    string Title,
    string Description,
    Difficulty Difficulty,
    int MemoryLimitMb,
    int CpuLimitMs,
    IEnumerable<TestCaseRequestDto> TestCases) : IRequest<Guid>;