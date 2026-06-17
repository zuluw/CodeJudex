namespace CodeJudex.Audit.Application.DTOs.Requests;

/// <summary>
/// DTO for a code audit request
/// </summary>
public record AuditRequestDto(string SourceCode, string Language = "csharp");