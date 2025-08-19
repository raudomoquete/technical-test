namespace DGII.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Request
        _logger.LogInformation($"{DateTime.Now}: Handling {typeof(TRequest).Name}");
        IList<PropertyInfo> props = new List<PropertyInfo>(request.GetType().GetProperties());
        foreach (PropertyInfo prop in props)
        {
            object propValue = prop.GetValue(request, null);
            _logger.LogInformation("{Property} : {@Value}", prop.Name, propValue);
        }
        var response = await next();

        //Response
        _logger.LogInformation($"{DateTime.Now}: Handled {typeof(TResponse).Name}");
        return response;
    }
}
