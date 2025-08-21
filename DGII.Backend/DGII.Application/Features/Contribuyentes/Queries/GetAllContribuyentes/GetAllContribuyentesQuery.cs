using MediatR;
using ErrorOr;

namespace DGII.Application.Features.Contribuyentes.Queries.GetAllContribuyentes;

public record GetAllContribuyentesQuery : IRequest<ErrorOr<IEnumerable<ContribuyenteDto>>>;
