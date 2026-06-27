using CodeJudex.Content.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Problem> Problems { get; }
    DbSet<TestCase> TestCases { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}