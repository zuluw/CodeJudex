using CodeJudex.Content.Domain.Enums;

namespace CodeJudex.Content.Domain.Models;

/// <summary>
/// Represents a technical challenge with its metadata and resource constraints.
/// </summary>
public class Problem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL-friendly identifier.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Difficulty Difficulty { get; set; }

    /// <summary>
    /// Gets or sets the maximum RAM allowed for the solution in megabytes.
    /// </summary>
    public int MemoryLimitMb { get; set; }

    /// <summary>
    /// Gets or sets the maximum execution time allowed in milliseconds.
    /// </summary>
    public int CpuLimitMs { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the collection of test cases used to verify the solution.
    /// </summary>
    public ICollection<TestCase> TestCases { get; set; } = [];
}