using CodeJudex.Audit.Application.DTOs.Requests;
using CodeJudex.Audit.Application.DTOs.Responses;
using CodeJudex.Audit.Application.Interfaces;
using CodeJudex.Audit.Application.Mappings;
using CodeJudex.Audit.Domain.Exceptions;
using CodeJudex.Audit.Domain.Models;
using CodeJudex.Audit.Domain.Rules;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CodeJudex.Audit.Application.Services;

public class AuditService(
    ICodeParser parser,
    IEnumerable<IAuditRule> rules,
    AuditMapper mapper,
    IValidator<AuditRequestDto> validator,
    ILogger<AuditService> logger) : IAuditService
{
    /// <inheritdoc />
    public async Task<AuditResponseDto> AuditCodeAsync(AuditRequestDto request, CancellationToken ct)
    {
        logger.LogInformation("Starting code audit. Language: {Language}", request.Language);

        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            logger.LogWarning("Request validation failed: {Errors}", errors);
            throw new BadRequestException(errors);
        }

        var rootNode = parser.Parse(request.SourceCode);

        var allIssues = new List<AuditIssue>();
        foreach (var rule in rules)
        {
            var ruleIssues = rule.Analyze(rootNode);
            allIssues.AddRange(ruleIssues);
        }

        var qualityScore = CalculateScore(allIssues);

        var domainResult = new AuditResult(
            qualityScore,
            allIssues.AsReadOnly(),
            DateTimeOffset.UtcNow
        );

        logger.LogInformation("Audit completed. Issues found: {Count}. Final score: {Score}", allIssues.Count, qualityScore);

        return mapper.MapToResponse(domainResult);
    }

    private static int CalculateScore(List<AuditIssue> issues)
    {
        var score = 100;
        foreach (var issue in issues)
        {
            score -= issue.Severity switch
            {
                Domain.Enums.Severity.Error => 20,
                Domain.Enums.Severity.Warning => 5,
                _ => 0
            };
        }

        return Math.Max(0, score);
    }
}