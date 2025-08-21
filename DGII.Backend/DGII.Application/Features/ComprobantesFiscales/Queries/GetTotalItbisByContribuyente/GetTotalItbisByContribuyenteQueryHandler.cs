using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using DGII.Domain.Errors;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetTotalItbisByContribuyente;

public class GetTotalItbisByContribuyenteQueryHandler : IRequestHandler<GetTotalItbisByContribuyenteQuery, ErrorOr<TotalItbisByContribuyenteDto>>
{
    private readonly IComprobanteFiscalRepository _comprobanteFiscalRepository;
    private readonly IContribuyenteRepository _contribuyenteRepository;
    private readonly ILogger<GetTotalItbisByContribuyenteQueryHandler> _logger;

    public GetTotalItbisByContribuyenteQueryHandler(
        IComprobanteFiscalRepository comprobanteFiscalRepository,
        IContribuyenteRepository contribuyenteRepository,
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
            var contribuyente = await _contribuyenteRepository.GetByRncCedulaAsync(request.RncCedula);

            if (contribuyente == null)
            {
                _logger.LogWarning("No se encontró el contribuyente {RncCedula}", request.RncCedula);
                return DgiiErrors.Contribuyente.NotFound(request.RncCedula);
            }

            // Calcular el total de ITBIS usando el método específico del repositorio
            var totalItbis = await _comprobanteFiscalRepository.GetTotalItbisByContribuyenteRncCedulaAsync(request.RncCedula);

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
            return DgiiErrors.General.DatabaseConnection;
        }
    }
}
