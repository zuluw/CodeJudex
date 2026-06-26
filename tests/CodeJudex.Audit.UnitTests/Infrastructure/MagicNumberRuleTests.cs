using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Infrastructure.Rules;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeJudex.Audit.UnitTests.Infrastructure;

public class MagicNumberRuleTests
{
    private readonly MagicNumberRule _rule = new();

    [Fact]
    public void Analyze_WhenMagicNumberUsedInExpression_ShouldReturnInfo()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    int result = 42 * 10;
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(x => x.RuleId == _rule.RuleId).Should().BeTrue();
        result.All(x => x.Severity == Severity.Info).Should().BeTrue();
    }

    [Theory]
    [InlineData("int x = 0;")]
    [InlineData("int y = 1;")]
    public void Analyze_WhenAllowedNumbersUsed_ShouldReturnNoIssues(string statement)
    {
        // Arrange
        var code = $"class Test {{ void Method() {{ {statement} }} }}";
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WhenNumberInConstDeclaration_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = """
            class Test {
                const int MaxRetry = 5;
                void Method() {
                    const int LocalLimit = 100;
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WhenNumberInEnum_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = """
            enum Status {
                Active = 1,
                Pending = 2,
                Deleted = 99
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WhenUsedInCondition_ShouldReturnInfoWithCorrectLine()
    {
        // Arrange
        const string code = """
            class Test {
                void Method(int age) {
                    if (age > 18) { } 
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().ContainSingle();
        result[0].Message.Should().Contain("18");
        result[0].LineNumber.Should().Be(3);
    }
}