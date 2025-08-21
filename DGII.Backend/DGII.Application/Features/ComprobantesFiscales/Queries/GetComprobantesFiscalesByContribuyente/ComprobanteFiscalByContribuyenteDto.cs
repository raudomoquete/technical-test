namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetComprobantesFiscalesByContribuyente;

public record ComprobanteFiscalByContribuyenteDto(
    string rncCedula,
    string NCF,
    string monto,
    string itbis18
);
