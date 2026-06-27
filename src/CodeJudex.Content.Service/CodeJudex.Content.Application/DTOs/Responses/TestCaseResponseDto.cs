namespace CodeJudex.Content.Application.DTOs.Responses;

/// <summary>
/// Represents a public test case example for the user.
/// </summary>
public record TestCaseResponseDto(
    string Input,
    string ExpectedOutput
);