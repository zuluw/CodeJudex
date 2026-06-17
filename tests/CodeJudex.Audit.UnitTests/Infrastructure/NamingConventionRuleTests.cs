using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Infrastructure.Rules;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeJudex.Audit.UnitTests.Infrastructure;

public class NamingConventionRuleTests
{
    private readonly NamingConventionRule _rule = new();

    [Theory]
    [InlineData("void badMethod() {}")] 
    [InlineData("void _InternalMethod() {}")]
    public void Analyze_WhenMethodNameIsInvalid_ShouldReturnWarning(string methodCode)
    {
        // Arrange
        var code = $"class Test {{ {methodCode} }}";
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(Severity.Warning);
    }

    [Fact]
    public void Analyze_WhenMethodNameIsPascalCase_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = "class Test { void ValidMethodName() {} }";
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WhenMultipleMethodsHaveInvalidNames_ShouldReturnAllIssues()
    {
        // Arrange
        const string code = """
            class Test {
                void firstMethod() {}
                void SecondMethod() {}
                void _thirdMethod() {}
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(x => x.RuleId == _rule.RuleId);
    }

    [Theory]
    [InlineData("void M() {}")]           
    [InlineData("void Method1() {}")]     
    [InlineData("void XMLParser() {}")]    
    public void Analyze_WhenMethodNameIsCorrect_ShouldReturnNoIssues(string methodCode)
    {
        // Arrange
        var code = $"class Test {{ {methodCode} }}";
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Analyze_WhenOtherMembersExist_ShouldIgnoreThem()
    {
        // Arrange
        const string code = """
            class badClassName {
                public badClassName() {} 
                public int property { get; set; } 
                void validMethod() {}
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().ContainSingle()
            .Which.Message.Should().Contain("validMethod");
    }

    [Fact]
    public async Task Analyze_WhenAsyncMethodNameIsInvalid_ShouldReturnWarning()
    {
        // Arrange
        const string code = "class Test { async Task startAsync() => await Task.CompletedTask; }";
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().ContainSingle()
            .Which.Message.Should().Contain("startAsync");
    }
}