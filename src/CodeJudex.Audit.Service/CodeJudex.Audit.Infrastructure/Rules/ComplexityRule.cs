using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeJudex.Audit.Infrastructure.Rules;

/// <summary>
/// Analyzes the nesting depth of control structures such as if, for, while, and switch.
/// </summary>
public class ComplexityRule : IAuditRule
{
    /// <inheritdoc />
    public string RuleId => "CJ-002";

    /// <inheritdoc />
    public string Title => "Excessive code nesting";

    private const int MaxNestingDepth = 3;

    /// <inheritdoc />
    public IEnumerable<AuditIssue> Analyze(SyntaxNode root)
    {
        var issues = new List<AuditIssue>();

        var nestingNodes = root.DescendantNodes().Where(IsControlStructure);

        foreach (var node in nestingNodes)
        {
            var depth = node.Ancestors().Count(IsControlStructure);

            if (depth >= MaxNestingDepth)
            {
                var lineSpan = node.GetLocation().GetLineSpan();

                issues.Add(new AuditIssue(
                    RuleId,
                    $"Nesting is too deep (level {depth + 1}). Consider refactoring to reduce complexity.",
                    Severity.Warning,
                    lineSpan.StartLinePosition.Line + 1
                ));
            }
        }

        return issues;
    }

    private static bool IsControlStructure(SyntaxNode node) =>
        node is IfStatementSyntax ||
        node is ForStatementSyntax ||
        node is ForEachStatementSyntax ||
        node is WhileStatementSyntax ||
        node is SwitchStatementSyntax;
}