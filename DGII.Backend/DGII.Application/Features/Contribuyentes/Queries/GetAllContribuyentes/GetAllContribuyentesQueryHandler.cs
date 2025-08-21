using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Features.Contribuyentes.Queries.GetAllContribuyentes;

public class GetAllContribuyentesQueryHandler : IRequestHandler<GetAllContribuyentesQuery, ErrorOr<IEnumerable<ContribuyenteDto>>>
{
    private readonly IContribuyenteRepository _contribuyenteRepository;
    private readonly ILogger<GetAllContribuyentesQueryHandler> _logger;

    public GetAllContribuyentesQueryHandler(
        IContribuyenteRepository contribuyenteRepository,
        ILogger<GetAllContribuyentesQueryHandler> logger)
    {
        _contribuyenteRepository = contribuyenteRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<IEnumerable<ContribuyenteDto>>> Handle(
        GetAllContribuyentesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Obteniendo listado de todos los contribuyentes");

            var contribuyentes = await _contribuyenteRepository.GetAllAsync();

            var contribuyentesDto = contribuyentes.Select(c => new ContribuyenteDto(
                rncCedula: c.rncCedula,
                nombre: c.nombre,
                tipo: c.tipo,
                estatus: c.estatus
            ));

            _logger.LogInformation("Se obtuvieron {Count} contribuyentes", contribuyentesDto.Count());

            return contribuyentesDto.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el listado de contribuyentes");
            return Error.Failure("Contribuyentes.GetAll", "Error al obtener el listado de contribuyentes");
        }
    }
}
