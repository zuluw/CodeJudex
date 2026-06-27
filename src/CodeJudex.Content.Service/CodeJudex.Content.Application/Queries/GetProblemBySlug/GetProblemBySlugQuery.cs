using CodeJudex.Content.Application.DTOs.Responses;
using MediatR;

namespace CodeJudex.Content.Application.Queries.GetProblemBySlug;

/// <summary>
/// Represents a query to retrieve a detailed problem view by its slug.
/// </summary>
/// <param name="Slug">The URL-friendly identifier of the problem.</param>
public record GetProblemBySlugQuery(string Slug) : IRequest<ProblemResponseDto>;