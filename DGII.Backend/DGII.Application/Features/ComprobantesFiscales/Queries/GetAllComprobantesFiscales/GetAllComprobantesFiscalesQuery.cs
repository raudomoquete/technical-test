using MediatR;
using ErrorOr;

namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetAllComprobantesFiscales;

public record GetAllComprobantesFiscalesQuery : IRequest<ErrorOr<IEnumerable<ComprobanteFiscalDto>>>;
