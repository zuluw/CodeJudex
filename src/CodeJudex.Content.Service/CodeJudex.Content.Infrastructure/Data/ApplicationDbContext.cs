using CodeJudex.Content.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.Infrastructure.Data;

/// <summary>
/// Provides access to the programming challenges and test cases in the database.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Problem> Problems => Set<Problem>();
    public DbSet<TestCase> TestCases => Set<TestCase>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Slug).IsUnique();

            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(250);
            entity.Property(e => e.Description).IsRequired();

            entity.HasMany(e => e.TestCases)
                .WithOne()
                .HasForeignKey(t => t.ProblemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TestCase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Input).IsRequired();
            entity.Property(e => e.ExpectedOutput).IsRequired();
        });
    }
}