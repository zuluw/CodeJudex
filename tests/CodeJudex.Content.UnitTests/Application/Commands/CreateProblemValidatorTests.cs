using CodeJudex.Content.Application.Commands.CreateProblem;
using CodeJudex.Content.Application.DTOs.Requests;
using CodeJudex.Content.Domain.Enums;
using FluentValidation.TestHelper;

namespace CodeJudex.Content.UnitTests.Application.Commands;

public class CreateProblemValidatorTests
{
    private readonly CreateProblemValidator _validator = new();

    [Fact]
    public void Validate_WhenFieldsAreEmpty_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new CreateProblemCommand("", "", Difficulty.Easy, 256, 1000, new List<TestCaseRequestDto>());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.TestCases);
    }

    [Fact]
    public void Validate_WhenTitleExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProblemCommand(new string('a', 201), "Valid description length here.", Difficulty.Easy, 256, 1000, new List<TestCaseRequestDto> { new("in", "out", false) });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WhenDescriptionIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProblemCommand("Valid Title", "Too short", Difficulty.Easy, 256, 1000, new List<TestCaseRequestDto> { new("in", "out", false) });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Theory]
    [InlineData(10)]  
    [InlineData(3000)] 
    public async Task Validate_WhenMemoryLimitIsOutOfRange_ShouldHaveValidationError(int memory)
    {
        // Arrange
        var command = new CreateProblemCommand("Valid Title", "Valid description length here.", Difficulty.Easy, memory, 1000, new List<TestCaseRequestDto> { new("in", "out", false) });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MemoryLimitMb);
    }

    [Theory]
    [InlineData(50)]    
    [InlineData(15000)] 
    public async Task Validate_WhenCpuLimitIsOutOfRange_ShouldHaveValidationError(int cpu)
    {
        // Arrange
        var command = new CreateProblemCommand("Valid Title", "Valid description length here.", Difficulty.Easy, 256, cpu, new List<TestCaseRequestDto> { new("in", "out", false) });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CpuLimitMs);
    }

    [Fact]
    public void Validate_WhenTestCasesListIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProblemCommand("Valid Title", "Valid description length here.", Difficulty.Easy, 256, 1000, new List<TestCaseRequestDto>());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TestCases)
              .WithErrorMessage("At least one test case is required.");
    }

    [Fact]
    public void Validate_WhenCommandIsValid_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateProblemCommand(
            "Two Sum",
            "The classic two sum challenge description.",
            Difficulty.Easy,
            256,
            1000,
            new List<TestCaseRequestDto> { new("in", "out", false) });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}