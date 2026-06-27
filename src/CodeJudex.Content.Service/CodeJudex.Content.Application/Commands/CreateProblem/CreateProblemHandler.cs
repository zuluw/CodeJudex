using CodeJudex.Content.Domain.Models;
using CodeJudex.Content.Application.Common.Interfaces;
using MediatR;
using System.Text.RegularExpressions;

namespace CodeJudex.Content.Application.Commands.CreateProblem;

/// <summary>
/// Handles the <see cref="CreateProblemCommand"/> to persist a new problem.
/// </summary>
public class CreateProblemHandler(IApplicationDbContext context) : IRequestHandler<CreateProblemCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> Handle(CreateProblemCommand request, CancellationToken ct)
    {
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = GenerateSlug(request.Title),
            Description = request.Description,
            Difficulty = request.Difficulty,
            MemoryLimitMb = request.MemoryLimitMb,
            CpuLimitMs = request.CpuLimitMs,
            TestCases = request.TestCases.Select(tc => new TestCase
            {
                Id = Guid.NewGuid(),
                Input = tc.Input,
                ExpectedOutput = tc.ExpectedOutput,
                IsHidden = tc.IsHidden
            }).ToList()
        };

        context.Problems.Add(problem);
        await context.SaveChangesAsync(ct);

        return problem.Id;
    }

    private static string GenerateSlug(string title)
    {
        var slug = title.ToLower().Trim();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        return slug;
    }
}