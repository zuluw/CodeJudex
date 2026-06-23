using CodeJudex.Identity.Application.Common.Interfaces;
using CodeJudex.Identity.Application.Common.Mappings;
using CodeJudex.Identity.Application.Features.Auth.Login;
using CodeJudex.Identity.Domain.Exceptions;
using CodeJudex.Identity.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CodeJudex.Identity.UnitTests.Application;

public class LoginHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _handler = new LoginHandler(
            _userManagerMock.Object,
            _tokenServiceMock.Object,
            new AuthMapper(),
            Mock.Of<ILogger<LoginHandler>>());
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var command = new LoginCommand("fake@test.com", "pass");
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((User?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WhenPasswordIsWrong_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var user = new User { Email = "user@test.com" };
        var command = new LoginCommand(user.Email, "wrong-pass");

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WhenCredentialsAreValid_ShouldReturnAuthResponse()
    {
        // Arrange
        var user = new User { Email = "user@test.com", FullName = "Test User" };
        var command = new LoginCommand(user.Email, "CorrectPass123!");
        var roles = new List<string> { "Student" };
        var expires = DateTime.UtcNow.AddMinutes(60);

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);

        _tokenServiceMock.Setup(x => x.GenerateTokens(user, roles))
            .Returns(("access", "refresh", expires));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("access");
        result.RefreshToken.Should().Be("refresh");
        result.Email.Should().Be(user.Email);
        _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }
}