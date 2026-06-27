using CodeJudex.Content.Application.Common.Mappings;
using CodeJudex.Content.Application.DTOs.Responses;
using CodeJudex.Content.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.Application.Queries.GetProblemsList;

/// <summary>
/// Handles the <see cref="GetProblemsListQuery"/> to fetch problem summaries.
/// </summary>
public class GetProblemsListHandler(IApplicationDbContext context, ProblemMapper mapper) : IRequestHandler<GetProblemsListQuery, IEnumerable<ProblemListDto>>
{
    /// <inheritdoc />
    public async Task<IEnumerable<ProblemListDto>> Handle(GetProblemsListQuery request, CancellationToken ct)
    {
        var problems = await context.Problems
            .AsNoTracking()
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(ct);

        return problems.Select(mapper.MapToListDto);
    }
}