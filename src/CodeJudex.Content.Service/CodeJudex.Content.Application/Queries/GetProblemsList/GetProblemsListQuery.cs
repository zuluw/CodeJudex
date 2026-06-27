using CodeJudex.Content.Application.DTOs.Responses;
using MediatR;

namespace CodeJudex.Content.Application.Queries.GetProblemsList;

/// <summary>
/// Represents a query to retrieve a list of all available problems.
/// </summary>
public record GetProblemsListQuery() : IRequest<IEnumerable<ProblemListDto>>;