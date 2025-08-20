
using ILogger = Serilog.ILogger;

namespace DGII.API.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ILogger _logger;
    protected readonly IMediator _mediator;

    public ApiController(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: statusCode, title: error.Description, detail: error.Code);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(modelStateDictionary);
    }
}
