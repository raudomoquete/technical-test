using MediatR;
using ErrorOr;

namespace DGII.Application.Features.ComprobantesFiscales.Commands.CreateComprobanteFiscal;

public record CreateComprobanteFiscalCommand(
    string RncCedula,
    string NCF,
    decimal Monto,
    decimal Itbis18
) : IRequest<ErrorOr<Guid>>;
