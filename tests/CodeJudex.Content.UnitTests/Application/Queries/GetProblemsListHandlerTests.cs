using CodeJudex.Content.Application.Common.Mappings;
using CodeJudex.Content.Application.Queries.GetProblemsList;
using CodeJudex.Content.Domain.Enums;
using CodeJudex.Content.Domain.Models;
using CodeJudex.Content.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.UnitTests.Application.Queries;

public class GetProblemsListHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly ProblemMapper _mapper;
    private readonly GetProblemsListHandler _handler;

    public GetProblemsListHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _mapper = new ProblemMapper();
        _handler = new GetProblemsListHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_WhenDatabaseHasProblems_ShouldReturnAllProblemsAsListDtos()
    {
        // Arrange
        var problems = new List<Problem>
        {
            new() { Id = Guid.NewGuid(), Title = "Task 1", Slug = "task-1", Difficulty = Difficulty.Easy },
            new() { Id = Guid.NewGuid(), Title = "Task 2", Slug = "task-2", Difficulty = Difficulty.Medium }
        };

        _context.Problems.AddRange(problems);
        await _context.SaveChangesAsync();

        var query = new GetProblemsListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Title == "Task 1" && p.Difficulty == Difficulty.Easy);
        result.Should().Contain(p => p.Title == "Task 2" && p.Difficulty == Difficulty.Medium);
    }

    [Fact]
    public async Task Handle_WhenDatabaseIsEmpty_ShouldReturnEmptyCollection()
    {
        // Arrange
        var query = new GetProblemsListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldOrderByCreatedAtDate()
    {
        // Arrange
        var firstProblem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "Old Task",
            Slug = "old",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };
        var secondProblem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "New Task",
            Slug = "new",
            CreatedAt = DateTime.UtcNow
        };

        _context.Problems.AddRange(secondProblem, firstProblem);
        await _context.SaveChangesAsync();

        var query = new GetProblemsListQuery();

        // Act
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        result.First().Title.Should().Be("Old Task");
        result.Last().Title.Should().Be("New Task");
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectDtoTypeWithoutHeavyFields()
    {
        // Arrange
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "Complex Task",
            Slug = "complex",
            Description = "This description should not be in the list DTO",
            TestCases = new List<TestCase> { new() { Input = "secret", ExpectedOutput = "secret" } }
        };

        _context.Problems.Add(problem);
        await _context.SaveChangesAsync();

        var query = new GetProblemsListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var dto = result.First();
        dto.Title.Should().Be("Complex Task");
        dto.Should().NotBeNull();
    }
}