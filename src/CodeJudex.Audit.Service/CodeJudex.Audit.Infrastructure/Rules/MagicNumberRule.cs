using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeJudex.Audit.Infrastructure.Rules;

/// <summary>
/// Detects numeric literals used directly in code instead of being defined as constants.
/// </summary>
public class MagicNumberRule : IAuditRule
{
    /// <inheritdoc />
    public string RuleId => "CJ-003";

    /// <inheritdoc />
    public string Title => "Magic number detected";

    private readonly HashSet<string> _allowedNumbers = ["0", "1"];

    /// <inheritdoc />
    public IEnumerable<AuditIssue> Analyze(SyntaxNode root)
    {
        var numericLiterals = root.DescendantNodes()
            .OfType<LiteralExpressionSyntax>()
            .Where(l => l.IsKind(SyntaxKind.NumericLiteralExpression));

        foreach (var literal in numericLiterals)
        {
            var value = literal.Token.Text;

            if (_allowedNumbers.Contains(value)) continue;

            if (IsInsideConstantDeclaration(literal)) continue;

            var lineSpan = literal.GetLocation().GetLineSpan();

            yield return new AuditIssue(
                RuleId,
                $"Magic number '{value}' detected. Consider using a named constant instead.",
                Severity.Info,
                lineSpan.StartLinePosition.Line + 1
            );
        }
    }

    private static bool IsInsideConstantDeclaration(SyntaxNode node)
    {
        return node.Ancestors().Any(a =>
            a is LocalDeclarationStatementSyntax localDecl && localDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.ConstKeyword)) ||
            a is FieldDeclarationSyntax fieldDecl && fieldDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.ConstKeyword)) ||
            a is EnumMemberDeclarationSyntax);
    }
}