namespace CodeJudex.Audit.Domain.Enums;

/// <summary>
/// The severity level of the code violation
/// </summary>
public enum Severity
{
    /// <summary>
    /// Recommendation for improvement (does not affect the score)
    /// </summary>
    Info,

    /// <summary>
    /// Warning (slight decrease in score)
    /// </summary>
    Warning,

    /// <summary>
    /// Critical error (serious violation of standards)
    /// </summary>
    Error
}