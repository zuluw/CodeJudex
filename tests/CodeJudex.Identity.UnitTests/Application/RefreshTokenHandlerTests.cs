using System.Security.Claims;
using CodeJudex.Identity.Application.Common.Interfaces;
using CodeJudex.Identity.Application.Common.Mappings;
using CodeJudex.Identity.Application.Features.Auth.RefreshToken;
using CodeJudex.Identity.Domain.Exceptions;
using CodeJudex.Identity.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CodeJudex.Identity.UnitTests.Application;

public class RefreshTokenHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly RefreshTokenHandler _handler;

    public RefreshTokenHandlerTests()
    {
        _userManagerMock = MockHelpers.GetUserManagerMock<User>();
        _handler = new RefreshTokenHandler(
            _userManagerMock.Object,
            _tokenServiceMock.Object,
            new AuthMapper(),
            Mock.Of<ILogger<RefreshTokenHandler>>());
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsInvalid_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var command = new RefreshTokenCommand("expired-access", "wrong-refresh");
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user@test.com") }));

        _tokenServiceMock.Setup(x => x.GetClaimsFromToken(command.AccessToken)).Returns(claims);
        _userManagerMock.Setup(x => x.FindByEmailAsync("user@test.com"))
            .ReturnsAsync(new User { RefreshToken = "actual-refresh" });

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldRotateTokensAndReturnResponse()
    {
        // Arrange
        var command = new RefreshTokenCommand("valid-but-expired-access", "valid-refresh");
        var user = new User { Email = "user@test.com", RefreshToken = "valid-refresh", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1) };
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Email) }));

        _tokenServiceMock.Setup(x => x.GetClaimsFromToken(command.AccessToken)).Returns(claims);
        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Student" });
        _tokenServiceMock.Setup(x => x.GenerateTokens(user, It.IsAny<IList<string>>()))
            .Returns(("new-access", "new-refresh", DateTime.UtcNow.AddMinutes(60)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("new-access");
        result.RefreshToken.Should().Be("new-refresh");
        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<User>(u => u.RefreshToken == "new-refresh")), Times.Once);
    }
}