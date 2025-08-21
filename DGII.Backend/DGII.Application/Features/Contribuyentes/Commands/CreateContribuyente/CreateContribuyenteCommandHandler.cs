using DGII.Application.Interfaces;
using DGII.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Features.Contribuyentes.Commands.CreateContribuyente;

public class CreateContribuyenteCommandHandler : IRequestHandler<CreateContribuyenteCommand, ErrorOr<Guid>>
{
    private readonly IRepository<Contribuyente> _contribuyenteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateContribuyenteCommandHandler> _logger;

    public CreateContribuyenteCommandHandler(
        IRepository<Contribuyente> contribuyenteRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateContribuyenteCommandHandler> logger)
    {
        _contribuyenteRepository = contribuyenteRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<Guid>> Handle(
        CreateContribuyenteCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creando contribuyente con RNC/Cédula: {RncCedula}", request.RncCedula);

            // Verificar si ya existe un contribuyente con el mismo RNC/Cédula
            var existingContribuyente = await _contribuyenteRepository.GetFirstOrDefaultAsync(
                predicate: c => c.rncCedula == request.RncCedula);

            if (existingContribuyente != null)
            {
                _logger.LogWarning("Ya existe un contribuyente con el RNC/Cédula: {RncCedula}", request.RncCedula);
                return Error.Conflict("Contribuyente.AlreadyExists", $"Ya existe un contribuyente con el RNC/Cédula {request.RncCedula}");
            }

            // Crear el nuevo contribuyente
            var contribuyente = new Contribuyente
            {
                Id = Guid.NewGuid(),
                rncCedula = request.RncCedula,
                nombre = request.Nombre,
                tipo = request.Tipo,
                estatus = request.Estatus
            };

            // Validar usando value objects
            contribuyente.SetRncCedula(request.RncCedula);

            // Agregar al repositorio
            await _contribuyenteRepository.InsertAsync(contribuyente);

            // Guardar cambios
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Contribuyente creado exitosamente con ID: {Id}", contribuyente.Id);

            return contribuyente.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear contribuyente con RNC/Cédula: {RncCedula}", request.RncCedula);
            return Error.Failure("Contribuyente.Create", "Error al crear el contribuyente");
        }
    }
}
