using CodeJudex.Identity.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace CodeJudex.Identity.Application.Common.Behaviors;

/// <summary>
/// Provides automatic validation for all MediatR requests.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, ct)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            throw new BadRequestException(string.Join(", ", failures.Select(f => f.ErrorMessage)));
        }

        return await next();
    }
}