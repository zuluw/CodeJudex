using CodeJudex.Audit.Application.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeJudex.Audit.Infrastructure.Parsers;

public class CSharpParser : ICodeParser
{
    /// <inheritdoc />
    public SyntaxNode Parse(string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        return syntaxTree.GetRoot();
    }
}