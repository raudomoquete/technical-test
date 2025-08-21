using DGII.Domain.Errors;

namespace DGII.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        ILogger<ValidationBehavior<TRequest, TResponse>> logger,
        IValidator<TRequest>? validator = null)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_validator is null)
        {
            return await next();
        }

        _logger.LogInformation("Validating request {RequestType}", typeof(TRequest).Name);
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            _logger.LogInformation("Validation passed for {RequestType}", typeof(TRequest).Name);
            return await next();
        }

        _logger.LogWarning("Validation failed for {RequestType} with {ErrorCount} errors", 
            typeof(TRequest).Name, validationResult.Errors.Count);

        var errors = validationResult
            .Errors
            .ConvertAll(
                validationFailure =>
                    DgiiErrors.General.Validation(
                        validationFailure.PropertyName, 
                        validationFailure.ErrorMessage
                    )
            );

        return (dynamic)errors;
    }
}
