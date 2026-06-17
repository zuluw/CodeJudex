using Microsoft.CodeAnalysis;
using CodeJudex.Audit.Domain.Models;

namespace CodeJudex.Audit.Domain.Rules;

public interface IAuditRule
{
    /// <summary>
    /// Gets the unique identifier for the rule (e.g., CJ-001).
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Gets the display title of the rule.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Analyzes the syntax tree for specific code violations.
    /// </summary>
    /// <param name="root">The root node of the C# syntax tree.</param>
    /// <returns>A collection of discovered <see cref="AuditIssue"/>.</returns>
    IEnumerable<AuditIssue> Analyze(SyntaxNode root);
}