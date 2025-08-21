using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetAllComprobantesFiscales;

public class GetAllComprobantesFiscalesQueryHandler : IRequestHandler<GetAllComprobantesFiscalesQuery, ErrorOr<IEnumerable<ComprobanteFiscalDto>>>
{
    private readonly IComprobanteFiscalRepository _comprobanteFiscalRepository;
    private readonly ILogger<GetAllComprobantesFiscalesQueryHandler> _logger;

    public GetAllComprobantesFiscalesQueryHandler(
        IComprobanteFiscalRepository comprobanteFiscalRepository,
        ILogger<GetAllComprobantesFiscalesQueryHandler> logger)
    {
        _comprobanteFiscalRepository = comprobanteFiscalRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<IEnumerable<ComprobanteFiscalDto>>> Handle(
        GetAllComprobantesFiscalesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obteniendo listado de todos los comprobantes fiscales");

            var comprobantesFiscales = await _comprobanteFiscalRepository.GetAllWithContribuyenteAsync();

            var comprobantesFiscalesDto = comprobantesFiscales.Select(cf => new ComprobanteFiscalDto(
                rncCedula: cf.Contribuyente.rncCedula,
                NCF: cf.NCF,
                monto: cf.monto.ToString("F2"),
                itbis18: cf.itbis18.ToString("F2")
            ));

            _logger.LogInformation("Se obtuvieron {Count} comprobantes fiscales", comprobantesFiscalesDto.Count());

            return comprobantesFiscalesDto.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el listado de comprobantes fiscales");
            return Error.Failure("ComprobantesFiscales.GetAll", "Error al obtener el listado de comprobantes fiscales");
        }
    }
}
