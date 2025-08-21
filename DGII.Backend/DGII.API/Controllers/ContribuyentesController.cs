using DGII.Application.Features.Contribuyentes.Queries.GetAllContribuyentes;
using DGII.Application.Features.ComprobantesFiscales.Queries.GetComprobantesFiscalesByContribuyente;
using DGII.Application.Features.ComprobantesFiscales.Queries.GetTotalItbisByContribuyente;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace DGII.API.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
public class ContribuyentesController : ApiController
{
    public ContribuyentesController(IMediator mediator, ILogger logger) 
        : base(mediator, logger)
    {
    }

    /// <summary>
    /// Obtiene el listado de todos los contribuyentes
    /// </summary>
    /// <returns>Lista de contribuyentes en formato JSON</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContribuyenteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllContribuyentes()
    {
        _logger.Information("Solicitud recibida para obtener todos los contribuyentes");

        var result = await _mediator.Send(new GetAllContribuyentesQuery());

        return result.Match(contribuyentes => Ok(contribuyentes), Problem);
    }

    /// <summary>
    /// Obtiene todos los comprobantes fiscales de un contribuyente específico
    /// </summary>
    /// <param name="rncCedula">RNC/Cédula del contribuyente</param>
    /// <returns>Lista de comprobantes fiscales del contribuyente en formato JSON</returns>
    [HttpGet("{rncCedula}/comprobantes")]
    [ProducesResponseType(typeof(IEnumerable<ComprobanteFiscalByContribuyenteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetComprobantesByContribuyente(string rncCedula)
    {
        if (string.IsNullOrWhiteSpace(rncCedula))
            return BadRequest("El RNC/Cédula es requerido");

        _logger.Information("Solicitud recibida para obtener comprobantes del contribuyente {RncCedula}", rncCedula);

        var result = await _mediator.Send(new GetComprobantesFiscalesByContribuyenteQuery(rncCedula));

        return result.Match(comprobantes => Ok(comprobantes), Problem);
    }

    /// <summary>
    /// Obtiene el total de ITBIS de todos los comprobantes fiscales de un contribuyente específico
    /// </summary>
    /// <param name="rncCedula">RNC/Cédula del contribuyente</param>
    /// <returns>Total de ITBIS del contribuyente en formato JSON</returns>
    [HttpGet("{rncCedula}/total-itbis")]
    [ProducesResponseType(typeof(TotalItbisByContribuyenteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTotalItbisByContribuyente(string rncCedula)
    {
        if (string.IsNullOrWhiteSpace(rncCedula))
            return BadRequest("El RNC/Cédula es requerido");

        _logger.Information("Solicitud recibida para obtener total ITBIS del contribuyente {RncCedula}", rncCedula);

        var result = await _mediator.Send(new GetTotalItbisByContribuyenteQuery(rncCedula));

        return result.Match(totalItbis => Ok(totalItbis), Problem);
    }
}
