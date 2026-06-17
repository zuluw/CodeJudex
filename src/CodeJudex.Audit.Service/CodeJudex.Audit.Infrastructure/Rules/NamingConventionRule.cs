using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeJudex.Audit.Infrastructure.Rules;

/// <summary>
/// Verifies that method names follow the PascalCase naming convention.
/// </summary>
public class NamingConventionRule : IAuditRule
{
    /// <inheritdoc />
    public string RuleId => "CJ-001";

    /// <inheritdoc />
    public string Title => "Method naming violation";

    /// <inheritdoc />
    public IEnumerable<AuditIssue> Analyze(SyntaxNode root)
    {
        var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methodDeclarations)
        {
            var methodName = method.Identifier.Text;

            if (!string.IsNullOrEmpty(methodName) && !char.IsUpper(methodName[0]))
            {
                var lineSpan = method.Identifier.GetLocation().GetLineSpan();

                yield return new AuditIssue(
                    RuleId,
                    $"Method '{methodName}' should start with an uppercase letter.",
                    Severity.Warning,
                    lineSpan.StartLinePosition.Line + 1
                );
            }
        }
    }
}