using MediatR;
using ErrorOr;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetComprobantesFiscalesByContribuyente;

public record GetComprobantesFiscalesByContribuyenteQuery(string RncCedula) : IRequest<ErrorOr<IEnumerable<ComprobanteFiscalByContribuyenteDto>>>;
