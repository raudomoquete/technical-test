using DGII.Application.Interfaces;
using DGII.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetComprobantesFiscalesByContribuyente;

public class GetComprobantesFiscalesByContribuyenteQueryHandler : IRequestHandler<GetComprobantesFiscalesByContribuyenteQuery, ErrorOr<IEnumerable<ComprobanteFiscalByContribuyenteDto>>>
{
    private readonly IRepository<ComprobanteFiscal> _comprobanteFiscalRepository;
    private readonly ILogger<GetComprobantesFiscalesByContribuyenteQueryHandler> _logger;

    public GetComprobantesFiscalesByContribuyenteQueryHandler(
        IRepository<ComprobanteFiscal> comprobanteFiscalRepository,
        ILogger<GetComprobantesFiscalesByContribuyenteQueryHandler> logger)
    {
        _comprobanteFiscalRepository = comprobanteFiscalRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<IEnumerable<ComprobanteFiscalByContribuyenteDto>>> Handle(
        GetComprobantesFiscalesByContribuyenteQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obteniendo comprobantes fiscales para el contribuyente {RncCedula}", request.RncCedula);

            var comprobantesFiscales = await _comprobanteFiscalRepository.GetAllAsync(
                predicate: cf => cf.Contribuyente.rncCedula == request.RncCedula,
                include: q => q.Include(cf => cf.Contribuyente));

            if (!comprobantesFiscales.Any())
            {
                _logger.LogWarning("No se encontraron comprobantes fiscales para el contribuyente {RncCedula}", request.RncCedula);
                return Error.NotFound("ComprobantesFiscales.NotFound", $"No se encontraron comprobantes fiscales para el contribuyente {request.RncCedula}");
            }

            var comprobantesFiscalesDto = comprobantesFiscales.Select(cf => new ComprobanteFiscalByContribuyenteDto(
                rncCedula: cf.Contribuyente.rncCedula,
                NCF: cf.NCF,
                monto: cf.monto.ToString("F2"),
                itbis18: cf.itbis18.ToString("F2")
            ));

            _logger.LogInformation("Se obtuvieron {Count} comprobantes fiscales para el contribuyente {RncCedula}", 
                comprobantesFiscalesDto.Count(), request.RncCedula);

            return comprobantesFiscalesDto.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener comprobantes fiscales para el contribuyente {RncCedula}", request.RncCedula);
            return Error.Failure("ComprobantesFiscales.GetByContribuyente", "Error al obtener comprobantes fiscales por contribuyente");
        }
    }
}
