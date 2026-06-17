using Microsoft.CodeAnalysis;

namespace CodeJudex.Audit.Application.Interfaces;

/// <summary>
/// Defines a service for parsing source code into a syntax tree.
/// </summary>
public interface ICodeParser
{
    /// <summary>
    /// Parses source code and returns the root node of the syntax tree.
    /// </summary>
    SyntaxNode Parse(string code);
}