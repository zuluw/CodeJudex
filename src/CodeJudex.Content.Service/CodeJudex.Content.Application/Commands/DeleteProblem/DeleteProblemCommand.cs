using MediatR;

namespace CodeJudex.Content.Application.Commands.DeleteProblem;

public record DeleteProblemCommand(Guid Id) : IRequest<bool>;