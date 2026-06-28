using CodeJudex.Content.Application.Common.Mappings;
using CodeJudex.Content.Application.Queries.GetProblemBySlug;
using CodeJudex.Content.Domain.Enums;
using CodeJudex.Content.Domain.Exceptions;
using CodeJudex.Content.Domain.Models;
using CodeJudex.Content.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.UnitTests.Application.Queries;

public class GetProblemBySlugHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly ProblemMapper _mapper;
    private readonly GetProblemBySlugHandler _handler;

    public GetProblemBySlugHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _mapper = new ProblemMapper();
        _handler = new GetProblemBySlugHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_WhenProblemExists_ShouldReturnDetailedDto()
    {
        // Arrange
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "Two Sum",
            Slug = "two-sum",
            Description = "Full task description here...",
            Difficulty = Difficulty.Easy,
            MemoryLimitMb = 256,
            CpuLimitMs = 1000,
            TestCases = new List<TestCase>
            {
                new() { Input = "in1", ExpectedOutput = "out1", IsHidden = false }
            }
        };

        _context.Problems.Add(problem);
        await _context.SaveChangesAsync();

        var query = new GetProblemBySlugQuery("two-sum");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(problem.Id);
        result.Slug.Should().Be("two-sum");
        result.Title.Should().Be("Two Sum");
        result.TestCases.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_WhenProblemDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetProblemBySlugQuery("non-existent-slug");

        // Act
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*non-existent-slug*");
    }

    [Fact]
    public async Task Handle_WhenSlugHasMixedCase_ShouldStillFindProblem()
    {
        // Arrange
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "Mixed Case Task",
            Slug = "mixed-case-task",
            Description = "Description long enough for the mapper."
        };

        _context.Problems.Add(problem);
        await _context.SaveChangesAsync();

        var query = new GetProblemBySlugQuery("MiXeD-CaSe-TaSk");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Slug.Should().Be("mixed-case-task");
    }

    [Fact]
    public async Task Handle_WhenProblemHasHiddenTests_ShouldReturnOnlyPublicTests()
    {
        // Arrange
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "Security Task",
            Slug = "security-task",
            Description = "Testing test case filtering logic.",
            TestCases = new List<TestCase>
            {
                new() { Input = "public-in", ExpectedOutput = "out", IsHidden = false },
                new() { Input = "hidden-in", ExpectedOutput = "out", IsHidden = true }
            }
        };

        _context.Problems.Add(problem);
        await _context.SaveChangesAsync();

        var query = new GetProblemBySlugQuery("security-task");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TestCases.Should().HaveCount(1);
        result.TestCases.Should().ContainSingle(tc => tc.Input == "public-in");
        result.TestCases.Should().NotContain(tc => tc.Input == "hidden-in");
    }

    [Fact]
    public async Task Handle_WhenProblemHasNoTestCases_ShouldReturnEmptyListInDto()
    {
        // Arrange
        var problem = new Problem
        {
            Id = Guid.NewGuid(),
            Title = "Empty Task",
            Slug = "empty-task",
            Description = "Task without any test cases recorded.",
            TestCases = new List<TestCase>()
        };

        _context.Problems.Add(problem);
        await _context.SaveChangesAsync();

        var query = new GetProblemBySlugQuery("empty-task");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TestCases.Should().BeEmpty();
    }
}