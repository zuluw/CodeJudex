using CodeJudex.Content.Application.DTOs.Requests;
using CodeJudex.Content.Domain.Enums;
using MediatR;

namespace CodeJudex.Content.Application.Commands.UpdateProblem;

public record UpdateProblemCommand(
    Guid Id,
    string Title,
    string Description,
    Difficulty Difficulty,
    int MemoryLimitMb,
    int CpuLimitMs,
    IEnumerable<TestCaseRequestDto> TestCases) : IRequest<bool>;