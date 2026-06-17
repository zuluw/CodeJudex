namespace CodeJudex.Audit.Domain.Enums;

public enum Severity
{
    /// <summary>
    /// Informational recommendation that does not affect the final score.
    /// </summary>
    Info,

    /// <summary>
    /// Warning indicating a minor violation. Reduces the score slightly.
    /// </summary>
    Warning,

    /// <summary>
    /// Critical error indicating a serious violation. Significantly reduces the score.
    /// </summary>
    Error
}