using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeJudex.Audit.Infrastructure.Rules;

/// <summary>
/// Detects string concatenation using '+=' inside loops by verifying variable types.
/// </summary>
public class StringBuilderRule : IAuditRule
{
    /// <inheritdoc />
    public string RuleId => "CJ-004";

    /// <inheritdoc />
    public string Title => "Inefficient string concatenation";

    /// <inheritdoc />
    public IEnumerable<AuditIssue> Analyze(SyntaxNode root)
    {
        var loops = root.DescendantNodes().Where(IsLoopStatement);

        foreach (var loop in loops)
        {
            var assignments = loop.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Where(a => a.IsKind(SyntaxKind.AddAssignmentExpression));

            foreach (var assignment in assignments)
            {
                if (IsStringAssignment(assignment, root))
                {
                    var lineSpan = assignment.OperatorToken.GetLocation().GetLineSpan();

                    yield return new AuditIssue(
                        RuleId,
                        "String concatenation detected inside a loop. Use 'StringBuilder' to avoid O(n^2) complexity.",
                        Severity.Warning,
                        lineSpan.StartLinePosition.Line + 1
                    );
                }
            }
        }
    }

    private static bool IsStringAssignment(AssignmentExpressionSyntax assignment, SyntaxNode root)
    {
        if (assignment.Left is not IdentifierNameSyntax identifier) return false;
        var variableName = identifier.Identifier.Text;

        var declaration = root.DescendantNodes()
            .OfType<VariableDeclarationSyntax>()
            .FirstOrDefault(d => d.Variables.Any(v => v.Identifier.Text == variableName));

        if (declaration == null) return false;

        var typeName = declaration.Type.ToString();
      
        if (typeName == "string" || typeName == "String") return true;
        
        if (typeName == "var")
        {
            var variable = declaration.Variables.First(v => v.Identifier.Text == variableName);
            return variable.Initializer?.Value is LiteralExpressionSyntax literal &&
                   literal.IsKind(SyntaxKind.StringLiteralExpression);
        }

        return false;
    }

    private static bool IsLoopStatement(SyntaxNode node) =>
        node is ForStatementSyntax || 
        node is ForEachStatementSyntax ||
        node is WhileStatementSyntax || 
        node is DoStatementSyntax;
}