namespace CodeJudex.Content.Application.DTOs.Requests;

/// <summary>
/// Represents the data required to create a new test case.
/// </summary>
public record TestCaseRequestDto(
    string Input,
    string ExpectedOutput,
    bool IsHidden
);