using FluentValidation;
using CodeJudex.Audit.Application.DTOs.Requests;

namespace CodeJudex.Audit.Application.Validators;

public class AuditRequestValidator : AbstractValidator<AuditRequestDto>
{
    public AuditRequestValidator()
    {      
        RuleFor(x => x.SourceCode)
            .NotEmpty().WithMessage("The source code cannot be empty.")
            .MinimumLength(5).WithMessage("The source code is too short for analysis.")
            .MaximumLength(100_000).WithMessage("The source code exceeds the maximum allowed size (100 KB).");

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("The programming language must be specified.")
            .Must(lang => lang.Equals("csharp", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Currently, only code audit for the C# language is supported.");
    }
}