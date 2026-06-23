using CodeJudex.Identity.Application.Features.Auth.Register;
using CodeJudex.Identity.Domain.Enums;
using FluentValidation.TestHelper;

namespace CodeJudex.Identity.UnitTests.Application;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator = new();

    [Fact]
    public void Validate_WhenFieldsAreEmpty_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new RegisterCommand("", "", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData("short")] 
    [InlineData("onlylowercase")] 
    [InlineData("ONLYUPPERCASE")] 
    [InlineData("NoNumberButCase")] 
    public void Validate_WhenPasswordDoesNotMatchPattern_ShouldHaveValidationError(string password)
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", password, "Full Name");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WhenRoleIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "Name", (UserRoles)999);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }
}