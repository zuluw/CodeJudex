using CodeJudex.Identity.Application.Common.Interfaces;
using CodeJudex.Identity.Application.Common.Mappings;
using CodeJudex.Identity.Application.Features.Auth.Register;
using CodeJudex.Identity.Domain.Exceptions;
using CodeJudex.Identity.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CodeJudex.Identity.UnitTests.Application;

public class RegisterHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly RegisterHandler _handler;

    public RegisterHandlerTests()
    {
        _userManagerMock = MockHelpers.GetUserManagerMock<User>();
        _roleManagerMock = MockHelpers.GetRoleManagerMock<Role>();

        _handler = new RegisterHandler(
            _userManagerMock.Object,
            _roleManagerMock.Object,
            _tokenServiceMock.Object,
            new AuthMapper(),
            Mock.Of<ILogger<RegisterHandler>>());
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyExists_ShouldThrowBadRequestException()
    {
        // Arrange
        var command = new RegisterCommand("exists@test.com", "Pass123!", "Name");
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(new User());

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>().WithMessage("User with this email already exists.");
    }

    [Fact]
    public async Task Handle_WhenIdentityResultFails_ShouldThrowBadRequestExceptionWithDetails()
    {
        // Arrange
        var command = new RegisterCommand("new@test.com", "Pass123!", "Name");
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((User?)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Identity Error" }));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Identity Error");
    }
}