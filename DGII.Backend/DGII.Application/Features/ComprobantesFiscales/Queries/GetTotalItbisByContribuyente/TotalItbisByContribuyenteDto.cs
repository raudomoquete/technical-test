namespace DGII.Application.Features.ComprobantesFiscales.Queries.GetTotalItbisByContribuyente;

public record TotalItbisByContribuyenteDto(
    string rncCedula,
    string nombre,
    string totalItbis
);
