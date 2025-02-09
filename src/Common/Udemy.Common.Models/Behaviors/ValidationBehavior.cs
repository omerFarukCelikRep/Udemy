using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Udemy.Common.Models.Behaviors;
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next);

        if (!validators?.Any() is not true)
            await next().ConfigureAwait(false);

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(validators!.Select(v => v.ValidateAsync(context, cancellationToken)))
                                                         .ConfigureAwait(false);

        var failures = validationResults.Where(v => v.Errors.Count > 0)
                                        .SelectMany(v => v.Errors)
                                        .ToList();

        if (failures is { Count: > 0 })
            throw new ValidationException(failures);

        return await next().ConfigureAwait(false);
    }
}
