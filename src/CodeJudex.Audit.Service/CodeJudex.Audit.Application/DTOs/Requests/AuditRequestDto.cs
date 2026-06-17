namespace CodeJudex.Audit.Application.DTOs.Requests;

/// <summary>
/// Represents a request to perform a code audit.
/// </summary>
/// <param name="SourceCode">The source code to be analyzed.</param>
/// <param name="Language">The programming language of the source code.</param>
public record AuditRequestDto(string SourceCode, string Language = "csharp");