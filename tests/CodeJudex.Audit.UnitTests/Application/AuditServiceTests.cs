using CodeJudex.Audit.Application.DTOs.Requests;
using CodeJudex.Audit.Application.Interfaces;
using CodeJudex.Audit.Application.Mappings;
using CodeJudex.Audit.Application.Services;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using FluentAssertions;
using FluentValidation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;
using Moq;

namespace CodeJudex.Audit.UnitTests.Application;

public class AuditServiceTests
{
    private readonly Mock<ICodeParser> _parserMock = new();
    private readonly Mock<IAuditRule> _ruleMock = new();
    private readonly Mock<IValidator<AuditRequestDto>> _validatorMock = new();
    private readonly AuditService _service;

    public AuditServiceTests()
    {
        _service = new AuditService(
            _parserMock.Object,
            new[] { _ruleMock.Object },
            new AuditMapper(),
            _validatorMock.Object,
            Mock.Of<ILogger<AuditService>>());
    }

    [Fact]
    public async Task AuditCodeAsync_WhenIssuesFound_ShouldCalculateScoreCorrectly()
    {
        // Arrange
        var request = new AuditRequestDto("valid code", "csharp");
        var ct = CancellationToken.None;

        _validatorMock.Setup(v => v.ValidateAsync(request, ct))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _parserMock.Setup(p => p.Parse(It.IsAny<string>()))
            .Returns(CSharpSyntaxTree.ParseText("").GetRoot());

        var issues = new List<AuditIssue>
        {
            new("ERR01", "Test Error", Domain.Enums.Severity.Error, 1)
        };
        _ruleMock.Setup(r => r.Analyze(It.IsAny<Microsoft.CodeAnalysis.SyntaxNode>()))
            .Returns(issues);

        // Act
        var result = await _service.AuditCodeAsync(request, ct);

        // Assert
        result.QualityScore.Should().Be(80);
        result.Issues.Should().HaveCount(1);
    }

    [Fact]
    public async Task AuditCodeAsync_WhenValidationFails_ShouldThrowBadRequestException()
    {
        // Arrange
        var request = new AuditRequestDto("", "csharp");
        var ct = CancellationToken.None;

        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new("SourceCode", "Code is empty")
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, ct))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationFailures));

        // Act
        var act = () => _service.AuditCodeAsync(request, ct);

        // Assert
        await act.Should().ThrowAsync<CodeJudex.Audit.Domain.Exceptions.BadRequestException>();
        _parserMock.Verify(p => p.Parse(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task AuditCodeAsync_WhenTooManyIssues_ShouldNotReturnScoreBelowZero()
    {
        // Arrange
        var request = new AuditRequestDto("code", "csharp");
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _parserMock.Setup(p => p.Parse(It.IsAny<string>()))
            .Returns(CSharpSyntaxTree.ParseText("").GetRoot());

        var issues = Enumerable.Range(1, 10)
            .Select(i => new AuditIssue($"ID{i}", "Error", Domain.Enums.Severity.Error, i))
            .ToList();

        _ruleMock.Setup(r => r.Analyze(It.IsAny<Microsoft.CodeAnalysis.SyntaxNode>()))
            .Returns(issues);

        // Act
        var result = await _service.AuditCodeAsync(request, CancellationToken.None);

        // Assert
        result.QualityScore.Should().Be(0);
    }

    [Fact]
    public async Task AuditCodeAsync_WhenMixedSeveritiesExist_ShouldCalculateTotalScoreCorrectly()
    {
        // Arrange
        var request = new AuditRequestDto("code", "csharp");
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _parserMock.Setup(p => p.Parse(It.IsAny<string>()))
            .Returns(CSharpSyntaxTree.ParseText("").GetRoot());

        var issues = new List<AuditIssue>
        {
            new("E1", "Critical", Domain.Enums.Severity.Error, 1),   
            new("W1", "Warning", Domain.Enums.Severity.Warning, 2), 
            new("I1", "Info", Domain.Enums.Severity.Info, 3)        
        };
        _ruleMock.Setup(r => r.Analyze(It.IsAny<Microsoft.CodeAnalysis.SyntaxNode>()))
            .Returns(issues);

        // Act
        var result = await _service.AuditCodeAsync(request, CancellationToken.None);

        // Assert
        result.QualityScore.Should().Be(75);
    }
}