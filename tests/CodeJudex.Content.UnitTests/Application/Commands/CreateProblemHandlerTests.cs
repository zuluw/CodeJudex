using CodeJudex.Content.Application.Commands.CreateProblem;
using CodeJudex.Content.Application.DTOs.Requests;
using CodeJudex.Content.Domain.Enums;
using CodeJudex.Content.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.UnitTests.Application.Commands;

public class CreateProblemHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly CreateProblemHandler _handler;

    public CreateProblemHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new CreateProblemHandler(_context);
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldPersistAllFieldsCorrectly()
    {
        // Arrange
        var command = new CreateProblemCommand(
            "Valid Title",
            "This is a very long and detailed description for the task.",
            Difficulty.Medium,
            128,
            500,
            new List<TestCaseRequestDto> { new("input", "output", false) }
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await _context.Problems.Include(p => p.TestCases).FirstAsync(p => p.Id == result);
        saved.Title.Should().Be(command.Title);
        saved.Description.Should().Be(command.Description);
        saved.Difficulty.Should().Be(command.Difficulty);
        saved.MemoryLimitMb.Should().Be(command.MemoryLimitMb);
        saved.CpuLimitMs.Should().Be(command.CpuLimitMs);
        saved.TestCases.First().Input.Should().Be("input");
    }

    [Theory]
    [InlineData("Simple Title", "simple-title")]
    [InlineData("Title With 123 Numbers", "title-with-123-numbers")]
    [InlineData("Special !@# Characters", "special-characters")]
    [InlineData("   Leading Trailing   ", "leading-trailing")]
    [InlineData("Multiple    Spaces", "multiple-spaces")]
    public async Task Handle_WithVariousTitles_ShouldGenerateExpectedSlug(string title, string expectedSlug)
    {
        // Arrange
        var command = new CreateProblemCommand(title, "Valid Description Length", Difficulty.Easy, 256, 1000, new List<TestCaseRequestDto>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await _context.Problems.FindAsync(result);
        saved!.Slug.Should().Be(expectedSlug);
    }

    [Theory]
    [InlineData(Difficulty.Easy)]
    [InlineData(Difficulty.Medium)]
    [InlineData(Difficulty.Hard)]
    public async Task Handle_WithDifferentDifficulties_ShouldSaveCorrectEnum(Difficulty difficulty)
    {
        // Arrange
        var command = new CreateProblemCommand("Title", "Description Description", difficulty, 256, 1000, new List<TestCaseRequestDto>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await _context.Problems.FindAsync(result);
        saved!.Difficulty.Should().Be(difficulty);
    }

    [Fact]
    public async Task Handle_WithMultipleTestCases_ShouldMaintainRelationshipAndOrder()
    {
        // Arrange
        var testCases = new List<TestCaseRequestDto>
        {
            new("in1", "out1", false),
            new("in2", "out2", true),
            new("in3", "out3", false)
        };
        var command = new CreateProblemCommand("Title", "Description Description", Difficulty.Easy, 256, 1000, testCases);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await _context.Problems.Include(p => p.TestCases).FirstAsync(p => p.Id == result);
        saved.TestCases.Should().HaveCount(3);
        saved.TestCases.All(tc => tc.ProblemId == saved.Id).Should().BeTrue();
        saved.TestCases.Should().Contain(tc => tc.ExpectedOutput == "out2" && tc.IsHidden);
    }

    [Fact]
    public async Task Handle_WhenCancellationTokenIsSignalled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var command = new CreateProblemCommand("Title", "Description Description", Difficulty.Easy, 256, 1000, new List<TestCaseRequestDto>());
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var act = () => _handler.Handle(command, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Handle_ShouldAssignUniqueGuidsToAllEntities()
    {
        // Arrange
        var testCases = new List<TestCaseRequestDto> { new("in1", "out1", false), new("in2", "out2", false) };
        var command = new CreateProblemCommand("Title", "Description Description", Difficulty.Easy, 256, 1000, testCases);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await _context.Problems.Include(p => p.TestCases).FirstAsync(p => p.Id == result);
        var ids = saved.TestCases.Select(tc => tc.Id).ToList();
        ids.Add(saved.Id);

        ids.Should().OnlyHaveUniqueItems();
        ids.All(id => id != Guid.Empty).Should().BeTrue();
    }
}