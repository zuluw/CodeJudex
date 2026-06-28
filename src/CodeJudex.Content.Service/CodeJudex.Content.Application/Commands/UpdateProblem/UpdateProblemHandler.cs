using CodeJudex.Content.Application.Common.Interfaces;
using CodeJudex.Content.Domain.Exceptions;
using CodeJudex.Content.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CodeJudex.Content.Application.Commands.UpdateProblem;

public class UpdateProblemHandler(IApplicationDbContext context) : IRequestHandler<UpdateProblemCommand, bool>
{
    public async Task<bool> Handle(UpdateProblemCommand request, CancellationToken ct)
    {
        var problem = await context.Problems
            .Include(p => p.TestCases)
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (problem == null)
        {
            throw new NotFoundException($"Problem with ID {request.Id} not found.");
        }

        problem.Title = request.Title;
        problem.Description = request.Description;
        problem.Difficulty = request.Difficulty;
        problem.MemoryLimitMb = request.MemoryLimitMb;
        problem.CpuLimitMs = request.CpuLimitMs;
        problem.Slug = GenerateSlug(request.Title);

        if (problem.TestCases.Any())
        {
            context.TestCases.RemoveRange(problem.TestCases);
        }

        var newTestCases = request.TestCases.Select(tc => new TestCase
        {
            Id = Guid.NewGuid(),
            ProblemId = problem.Id,
            Input = tc.Input,
            ExpectedOutput = tc.ExpectedOutput,
            IsHidden = tc.IsHidden
        }).ToList();

        await context.TestCases.AddRangeAsync(newTestCases, ct);
        await context.SaveChangesAsync(ct);

        return true;
    }

    private static string GenerateSlug(string title)
    {
        var slug = title.ToLower().Trim();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        return slug;
    }
}