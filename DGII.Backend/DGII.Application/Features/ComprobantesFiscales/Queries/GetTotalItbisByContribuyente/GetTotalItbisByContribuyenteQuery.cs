using MediatR;
using ErrorOr;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetTotalItbisByContribuyente;

public record GetTotalItbisByContribuyenteQuery(string RncCedula) : IRequest<ErrorOr<TotalItbisByContribuyenteDto>>;
