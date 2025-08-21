using DGII.Application.Interfaces;
using DGII.Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGII.Application.Features.ComprobantesFiscales.Commands.CreateComprobanteFiscal;

public class CreateComprobanteFiscalCommandHandler : IRequestHandler<CreateComprobanteFiscalCommand, ErrorOr<Guid>>
{
    private readonly IRepository<ComprobanteFiscal> _comprobanteFiscalRepository;
    private readonly IRepository<Contribuyente> _contribuyenteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateComprobanteFiscalCommandHandler> _logger;

    public CreateComprobanteFiscalCommandHandler(
        IRepository<ComprobanteFiscal> comprobanteFiscalRepository,
        IRepository<Contribuyente> contribuyenteRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateComprobanteFiscalCommandHandler> logger)
    {
        _comprobanteFiscalRepository = comprobanteFiscalRepository;
        _contribuyenteRepository = contribuyenteRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<Guid>> Handle(
        CreateComprobanteFiscalCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creando comprobante fiscal con NCF: {NCF} para contribuyente: {RncCedula}", 
                request.NCF, request.RncCedula);

            // Verificar que el contribuyente existe
            var contribuyente = await _contribuyenteRepository.GetFirstOrDefaultAsync(
                predicate: c => c.rncCedula == request.RncCedula);

            if (contribuyente == null)
            {
                _logger.LogWarning("No se encontró el contribuyente con RNC/Cédula: {RncCedula}", request.RncCedula);
                return Error.NotFound("Contribuyente.NotFound", $"No se encontró el contribuyente con RNC/Cédula {request.RncCedula}");
            }

            // Verificar si ya existe un comprobante fiscal con el mismo NCF
            var existingComprobante = await _comprobanteFiscalRepository.GetFirstOrDefaultAsync(
                predicate: cf => cf.NCF == request.NCF);

            if (existingComprobante != null)
            {
                _logger.LogWarning("Ya existe un comprobante fiscal con el NCF: {NCF}", request.NCF);
                return Error.Conflict("ComprobanteFiscal.AlreadyExists", $"Ya existe un comprobante fiscal con el NCF {request.NCF}");
            }

            // Crear el nuevo comprobante fiscal
            var comprobanteFiscal = new ComprobanteFiscal
            {
                Id = Guid.NewGuid(),
                ContribuyenteId = contribuyente.Id,
                NCF = request.NCF,
                monto = request.Monto,
                itbis18 = request.Itbis18
            };

            // Validar usando value objects
            comprobanteFiscal.SetNCF(request.NCF);
            comprobanteFiscal.SetMonto(request.Monto);
            comprobanteFiscal.SetItbis(request.Itbis18);

            // Agregar al repositorio
            await _comprobanteFiscalRepository.InsertAsync(comprobanteFiscal);

            // Guardar cambios
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Comprobante fiscal creado exitosamente con ID: {Id}", comprobanteFiscal.Id);

            return comprobanteFiscal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear comprobante fiscal con NCF: {NCF}", request.NCF);
            return Error.Failure("ComprobanteFiscal.Create", "Error al crear el comprobante fiscal");
        }
    }
}
