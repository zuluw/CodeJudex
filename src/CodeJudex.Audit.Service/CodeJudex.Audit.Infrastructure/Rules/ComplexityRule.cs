using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeJudex.Audit.Infrastructure.Rules;

/// <summary>
/// Analyzes the nesting depth of control structures (if, for, while, switch).
/// </summary>
public class ComplexityRule : IAuditRule
{
    public string RuleId => "CJ-002";
    public string Title => "Excessive code nesting";

    private const int MaxNestingDepth = 3;

    public IEnumerable<AuditIssue> Analyze(SyntaxNode root)
    {
        var issues = new List<AuditIssue>();

        var nestingNodes = root.DescendantNodes().Where(node =>
            node is IfStatementSyntax ||
            node is ForStatementSyntax ||
            node is ForEachStatementSyntax ||
            node is WhileStatementSyntax ||
            node is SwitchStatementSyntax);

        foreach (var node in nestingNodes)
        {            
            var depth = node.Ancestors().Count(ancestor =>
                ancestor is IfStatementSyntax ||
                ancestor is ForStatementSyntax ||
                ancestor is ForEachStatementSyntax ||
                ancestor is WhileStatementSyntax ||
                ancestor is SwitchStatementSyntax);

            if (depth >= MaxNestingDepth)
            {
                var lineSpan = node.GetLocation().GetLineSpan();
                var lineNumber = lineSpan.StartLinePosition.Line + 1;

                issues.Add(new AuditIssue(
                    RuleId,
                    $"Nesting is too deep (level {depth + 1}). It is recommended to refactor and extract the logic into separate methods.",
                    Severity.Warning,
                    lineNumber
                ));
            }
        }

        return issues;
    }
}