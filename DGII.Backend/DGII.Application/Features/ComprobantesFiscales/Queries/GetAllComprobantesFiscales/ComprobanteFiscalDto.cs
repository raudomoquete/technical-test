namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetAllComprobantesFiscales;

public record ComprobanteFiscalDto(
    string rncCedula,
    string NCF,
    string monto,
    string itbis18
);
