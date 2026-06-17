using FluentValidation;
using CodeJudex.Audit.Application.DTOs.Requests;

namespace CodeJudex.Audit.Application.Validators;

/// <summary>
/// Validator for checking the incoming code audit request
/// </summary>
public class AuditRequestValidator : AbstractValidator<AuditRequestDto>
{
    public AuditRequestValidator()
    {      
        RuleFor(x => x.SourceCode)
            .NotEmpty().WithMessage("The source code cannot be empty.")
            .MinimumLength(5).WithMessage("The source code is too short for analysis.")
            .MaximumLength(100_000).WithMessage("The source code exceeds the maximum allowed size (100 KB).");

        // currently, we only support C# code audits, so we validate the language field accordingly
        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("The programming language must be specified.")
            .Must(lang => lang.ToLower() == "csharp")
            .WithMessage("Currently, only code audit for the C# language is supported.");
    }
}