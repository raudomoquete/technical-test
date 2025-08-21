using DGII.Application.Interfaces;
using DGII.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetTotalItbisByContribuyente;

public class GetTotalItbisByContribuyenteQueryHandler : IRequestHandler<GetTotalItbisByContribuyenteQuery, ErrorOr<TotalItbisByContribuyenteDto>>
{
    private readonly IRepository<ComprobanteFiscal> _comprobanteFiscalRepository;
    private readonly IRepository<Contribuyente> _contribuyenteRepository;
    private readonly ILogger<GetTotalItbisByContribuyenteQueryHandler> _logger;

    public GetTotalItbisByContribuyenteQueryHandler(
        IRepository<ComprobanteFiscal> comprobanteFiscalRepository,
        IRepository<Contribuyente> contribuyenteRepository,
        ILogger<GetTotalItbisByContribuyenteQueryHandler> logger)
    {
        _comprobanteFiscalRepository = comprobanteFiscalRepository;
        _contribuyenteRepository = contribuyenteRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<TotalItbisByContribuyenteDto>> Handle(
        GetTotalItbisByContribuyenteQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Calculando total de ITBIS para el contribuyente {RncCedula}", request.RncCedula);

            // Verificar que el contribuyente existe
            var contribuyente = await _contribuyenteRepository.GetFirstOrDefaultAsync(
                predicate: c => c.rncCedula == request.RncCedula);

            if (contribuyente == null)
            {
                _logger.LogWarning("No se encontró el contribuyente {RncCedula}", request.RncCedula);
                return Error.NotFound("Contribuyente.NotFound", $"No se encontró el contribuyente {request.RncCedula}");
            }

            // Obtener todos los comprobantes fiscales del contribuyente
            var comprobantesFiscales = await _comprobanteFiscalRepository.GetAllAsync(
                predicate: cf => cf.ContribuyenteId == contribuyente.Id);

            // Calcular el total de ITBIS
            var totalItbis = comprobantesFiscales.Sum(cf => cf.itbis18);

            var result = new TotalItbisByContribuyenteDto(
                rncCedula: contribuyente.rncCedula,
                nombre: contribuyente.nombre,
                totalItbis: totalItbis.ToString("F2")
            );

            _logger.LogInformation("Total de ITBIS para el contribuyente {RncCedula}: ${TotalItbis}", 
                request.RncCedula, result.totalItbis);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al calcular el total de ITBIS para el contribuyente {RncCedula}", request.RncCedula);
            return Error.Failure("ComprobantesFiscales.GetTotalItbis", "Error al calcular el total de ITBIS por contribuyente");
        }
    }
}
