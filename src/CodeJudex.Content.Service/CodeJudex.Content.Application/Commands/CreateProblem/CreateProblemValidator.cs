using FluentValidation;

namespace CodeJudex.Content.Application.Commands.CreateProblem;

/// <summary>
/// Defines validation rules for the <see cref="CreateProblemCommand"/>.
/// </summary>
public class CreateProblemValidator : AbstractValidator<CreateProblemCommand>
{
    public CreateProblemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty().MinimumLength(20);

        RuleFor(x => x.MemoryLimitMb)
            .InclusiveBetween(16, 512);

        RuleFor(x => x.CpuLimitMs)
            .InclusiveBetween(100, 3000);

        RuleFor(x => x.TestCases)
            .NotEmpty().WithMessage("At least one test case is required.");
    }
}