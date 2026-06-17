using CodeJudex.Audit.Application.DTOs.Requests;
using CodeJudex.Audit.Application.Validators;
using FluentValidation.TestHelper;

namespace CodeJudex.Audit.UnitTests.Application;

public class AuditRequestValidatorTests
{
    private readonly AuditRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenSourceCodeIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var model = new AuditRequestDto("", "csharp");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SourceCode);
    }

    [Fact]
    public void Validate_WhenLanguageIsUnsupported_ShouldHaveValidationError()
    {
        // Arrange
        var model = new AuditRequestDto("void Method() {}", "unsupported-lang-123");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Language);
    }

    [Fact]
    public void Validate_WhenSourceCodeIsTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var model = new AuditRequestDto("void", "csharp");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SourceCode)
            .WithErrorMessage("The source code is too short for analysis.");
    }

    [Theory]
    [InlineData("csharp")]
    [InlineData("CSHARP")]
    [InlineData("CSharp")]
    public void Validate_WhenLanguageIsCSharpInAnyCase_ShouldNotHaveValidationError(string lang)
    {
        // Arrange
        var model = new AuditRequestDto("void Method() {}", lang);

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Language);
    }

    [Fact]
    public void Validate_WhenFieldsAreNull_ShouldHaveValidationErrors()
    {
        // Arrange
        var model = new AuditRequestDto(null!, null!);

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SourceCode);
        result.ShouldHaveValidationErrorFor(x => x.Language);
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var model = new AuditRequestDto("public class Test {}", "csharp");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}