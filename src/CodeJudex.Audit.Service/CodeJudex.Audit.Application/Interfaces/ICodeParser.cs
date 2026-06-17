using Microsoft.CodeAnalysis;

namespace CodeJudex.Audit.Application.Interfaces;

/// <summary>
/// Interface for a service that converts source code into an abstract syntax tree (AST)
/// </summary>
public interface ICodeParser
{
    /// <summary>
    /// Parses a source code string and returns the root of the syntax tree.
    /// </summary>
    /// <param name="code">Program code</param>
    /// <returns>Root of the syntax tree</returns>
    SyntaxNode Parse(string code);
}