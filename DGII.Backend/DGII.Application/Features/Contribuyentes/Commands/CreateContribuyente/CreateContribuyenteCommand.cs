using MediatR;
using ErrorOr;

namespace DGII.Application.Features.Contribuyentes.Commands.CreateContribuyente;

public record CreateContribuyenteCommand(
    string RncCedula,
    string Nombre,
    string Tipo,
    string Estatus
) : IRequest<ErrorOr<Guid>>;
