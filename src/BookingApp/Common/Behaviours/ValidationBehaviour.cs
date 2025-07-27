
using BookingApp.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace BookingApp.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ApplicationError(ErrorType.BadRequest, 
                    $"Validation failed for request {typeof(TRequest).Name}. errors: " +
                    string.Join(", ", failures.Select(f => f.ErrorMessage)));
            }

        }

        return await next(cancellationToken);
    }
}