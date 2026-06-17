using CodeJudex.Audit.Domain.Enums;
using CodeJudex.Audit.Infrastructure.Rules;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeJudex.Audit.UnitTests.Infrastructure;

public class ComplexityRuleTests
{
    private readonly ComplexityRule _rule = new();

    [Fact]
    public void Analyze_WhenNestingIsDeep_ShouldReturnWarning()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    if (true) {
                        if (true) {
                            if (true) {
                                if (true) { 
                                }
                            }
                        }
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
        result[0].Message.Should().Contain("level 4");
    }

    [Fact]
    public void Analyze_WhenNestingIsNormal_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    if (true) {
                        if (true) { 
                        }
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
    public void Analyze_WhenNestingIsExactlyAtLimit_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    if (true) {          
                        for (int i=0; i<1; i++) {
                            while (true) {
                            }
                        }
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
    public void Analyze_WhenMixedStructuresAreDeeplyNested_ShouldReturnWarning()
    {
        // Arrange
        const string code = """
            class Test {
                void Method() {
                    foreach (var x in list) { 
                        while (true) {        
                            switch (x) {      
                                case 1:
                                    if (true) { 
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().ContainSingle()
            .Which.Message.Should().Contain("level 4");
    }

    [Fact]
    public void Analyze_WhenMultipleViolationsExist_ShouldReturnAllIssues()
    {
        // Arrange
        const string code = """
            class Test {
                void Method1() {
                    if(true) if(true) if(true) if(true) {} 
                }
                void Method2() {
                    if(true) if(true) if(true) if(true) {}
                }
            }
            """;
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(x => x.RuleId == _rule.RuleId).Should().BeTrue();
    }

    [Fact]
    public void Analyze_WhenNoControlStructuresExist_ShouldReturnNoIssues()
    {
        // Arrange
        const string code = "class Test { void Method() { int x = 1 + 1; } }";
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();

        // Act
        var result = _rule.Analyze(root);

        // Assert
        result.Should().BeEmpty();
    }
}