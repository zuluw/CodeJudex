using CodeJudex.Content.Application.Common.Mappings;
using CodeJudex.Content.Application.DTOs.Responses;
using CodeJudex.Content.Domain.Exceptions;
using CodeJudex.Content.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.Application.Queries.GetProblemBySlug;

/// <summary>
/// Handles the <see cref="GetProblemBySlugQuery"/> to fetch a specific problem.
/// </summary>
public class GetProblemBySlugHandler(IApplicationDbContext context, ProblemMapper mapper) : IRequestHandler<GetProblemBySlugQuery, ProblemResponseDto>
{
    /// <inheritdoc />
    public async Task<ProblemResponseDto> Handle(GetProblemBySlugQuery request, CancellationToken ct)
    {
        var problem = await context.Problems
            .AsNoTracking()
            .Include(p => p.TestCases)
            .FirstOrDefaultAsync(p => p.Slug == request.Slug.ToLower(), ct);

        if (problem == null)
        {
            throw new NotFoundException($"Problem with slug '{request.Slug}' was not found.");
        }

        return mapper.MapToResponseDto(problem);
    }
}