using CodeJudex.Content.Application.Common.Interfaces;
using CodeJudex.Content.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeJudex.Content.Application.Commands.DeleteProblem;

public class DeleteProblemHandler(IApplicationDbContext context) : IRequestHandler<DeleteProblemCommand, bool>
{
    public async Task<bool> Handle(DeleteProblemCommand request, CancellationToken ct)
    {
        var problem = await context.Problems.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (problem == null) throw new NotFoundException("Problem not found");

        context.Problems.Remove(problem);
        await context.SaveChangesAsync(ct);
        return true;
    }
}