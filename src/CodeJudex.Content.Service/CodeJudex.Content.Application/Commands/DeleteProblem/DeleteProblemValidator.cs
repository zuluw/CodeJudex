using FluentValidation;

namespace CodeJudex.Content.Application.Commands.DeleteProblem;

public class DeleteProblemValidator : AbstractValidator<DeleteProblemCommand>
{
    public DeleteProblemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("A valid Problem ID must be provided for deletion.");
    }
}