namespace CodeJudex.Content.Domain.Models;

/// <summary>
/// Represents input and expected output data for code verification.
/// </summary>
public class TestCase
{
    public Guid Id { get; set; }

    public Guid ProblemId { get; set; }

    /// <summary>
    /// Gets or sets the raw input data for the test.
    /// </summary>
    public string Input { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expected result to compare against the solution output.
    /// </summary>
    public string ExpectedOutput { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the test case is hidden from the user.
    /// </summary>
    public bool IsHidden { get; set; }
}