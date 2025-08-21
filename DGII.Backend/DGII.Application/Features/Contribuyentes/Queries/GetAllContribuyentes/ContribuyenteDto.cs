namespace DGII.Application.Features.Contribuyentes.Queries.GetAllContribuyentes;

public record ContribuyenteDto(
    string rncCedula,
    string nombre,
    string tipo,
    string estatus
);
