using Microsoft.CodeAnalysis;
using CodeJudex.Audit.Domain.Models;

namespace CodeJudex.Audit.Domain.Rules;

/// <summary>
/// Interface for implementing automated code audit rules
/// </summary>
public interface IAuditRule
{
    /// <summary>
    /// Unique rule identifier (e.g., CJ001)
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Rule title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Method for analyzing the syntax tree
    /// </summary>
    /// <param name="root">Root of the syntax tree</param>
    /// <returns>List of found violations</returns>
    IEnumerable<AuditIssue> Analyze(SyntaxNode root);
}