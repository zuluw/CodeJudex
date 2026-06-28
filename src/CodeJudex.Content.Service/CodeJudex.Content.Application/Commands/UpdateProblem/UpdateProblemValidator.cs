using FluentValidation;

namespace CodeJudex.Content.Application.Commands.UpdateProblem;

public class UpdateProblemValidator : AbstractValidator<UpdateProblemCommand>
{
    public UpdateProblemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Problem ID is required for update.");

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