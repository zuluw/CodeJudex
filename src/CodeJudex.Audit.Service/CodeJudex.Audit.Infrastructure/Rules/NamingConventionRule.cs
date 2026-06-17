using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeJudex.Audit.Infrastructure.Rules;

/// <summary>
/// Verifies that method names use PascalCase formatting.
/// </summary>
public class NamingConventionRule : IAuditRule
{
    public string RuleId => "CJ-001";
    public string Title => "Method naming violation";

    public IEnumerable<AuditIssue> Analyze(SyntaxNode root)
    {
        var issues = new List<AuditIssue>();

        var methodDeclarations = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var method in methodDeclarations)
        {
            var methodName = method.Identifier.Text;

            if (!string.IsNullOrEmpty(methodName) && char.IsLower(methodName[0]))
            {
                var lineSpan = method.Identifier.GetLocation().GetLineSpan();
                var lineNumber = lineSpan.StartLinePosition.Line + 1;

                issues.Add(new AuditIssue(
                    RuleId,
                    $"Method '{methodName}' must start with an uppercase letter (PascalCase).",
                    Severity.Warning,
                    lineNumber
                ));
            }
        }

        return issues;
    }
}