using DGII.Application.Features.ComprobantesFiscales.Queries.GetAllComprobantesFiscales;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace DGII.API.Controllers;

[Route("api/comprobantes-fiscales")]
[Produces("application/json")]
public class ComprobantesFiscalesController : ApiController
{
    public ComprobantesFiscalesController(IMediator mediator, ILogger logger) 
        : base(mediator, logger)
    {
    }

    /// <summary>
    /// Obtiene el listado de todos los comprobantes fiscales
    /// </summary>
    /// <returns>Lista de comprobantes fiscales en formato JSON</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ComprobanteFiscalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllComprobantesFiscales()
    {
        _logger.Information("Solicitud recibida para obtener todos los comprobantes fiscales");

        var result = await _mediator.Send(new GetAllComprobantesFiscalesQuery());

        return result.Match(comprobantes => Ok(comprobantes), Problem);
    }
}
