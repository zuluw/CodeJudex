using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Infrastructure.Rules;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeJudex.Audit.UnitTests.Infrastructure;

public class StringBuilderRuleTests
{
    private readonly StringBuilderRule _rule = new();

    [Fact]
    public void Analyze_WhenStringConcatenatedInForLoop_ShouldReturnWarning()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    string s = "";
                    for (int i = 0; i < 10; i++) {
                        s += "data";
                    }
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().ContainSingle();
        result[0].RuleId.Should().Be(_rule.RuleId);
        result[0].Severity.Should().Be(Severity.Warning);
        result[0].Message.Should().Contain("StringBuilder");
    }

    [Fact]
    public void Analyze_WhenIntAddedInLoop_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    int sum = 0;
                    for (int i = 0; i < 10; i++) {
                        sum += i; 
                    }
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
    public void Analyze_WhenVarInitializedWithStringIsUsed_ShouldReturnWarning()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    var text = "start";
                    while (true) {
                        text += "!"; 
                    }
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().ContainSingle();
        result[0].Message.Should().Contain("StringBuilder");
    }

    [Fact]
    public void Analyze_WhenStringConcatenatedOutsideLoop_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    string s = "a";
                    s += "b"; 
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
    public void Analyze_WhenStringAddedInForEachLoop_ShouldReturnWarning()
    {
        // Arrange
        const string code = """
            class Test {
                void Method(string[] items) {
                    string result = "";
                    foreach (var item in items) {
                        result += item; 
                    }
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().NotBeEmpty();
    }
}