using CodeJudex.Identity.Application.Features.Auth.Login;
using FluentValidation.TestHelper;

namespace CodeJudex.Identity.UnitTests.Application;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public void Validate_WhenFieldsAreEmpty_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new LoginCommand("", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand("not-an-email", "Password123!");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenCommandIsValid_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new LoginCommand("user@test.com", "Password123!");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}