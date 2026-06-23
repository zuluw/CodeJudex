using CodeJudex.Identity.Application.Common.Behaviors;
using CodeJudex.Identity.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace CodeJudex.Identity.UnitTests.Application;

public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<TestRequest>> _validatorMock = new();
    private readonly Mock<RequestHandlerDelegate<TestResponse>> _nextMock = new();

    public record TestRequest : IRequest<TestResponse>;
    public record TestResponse;

    [Fact]
    public async Task Handle_WhenNoValidatorsExist_ShouldCallNext()
    {
        // Arrange
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(Enumerable.Empty<IValidator<TestRequest>>());

        // Act
        await behavior.Handle(new TestRequest(), _nextMock.Object, CancellationToken.None);

        // Assert
        _nextMock.Verify(x => x(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowBadRequestException()
    {
        // Arrange
        var failures = new List<ValidationFailure> { new("Property", "Error message") };
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { _validatorMock.Object });

        // Act
        var act = () => behavior.Handle(new TestRequest(), _nextMock.Object, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
        _nextMock.Verify(x => x(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidationSucceeds_ShouldCallNext()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { _validatorMock.Object });

        // Act
        await behavior.Handle(new TestRequest(), _nextMock.Object, CancellationToken.None);

        // Assert
        _nextMock.Verify(x => x(), Times.Once);
    }
}