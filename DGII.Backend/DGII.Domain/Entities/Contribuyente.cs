using DGII.Domain.ValueObjects;

namespace DGII.Domain.Entities;

public partial class Contribuyente
{
    public Guid Id { get; set; }

    public string rncCedula { get; set; } = null!;

    public string nombre { get; set; } = null!;

    public string tipo { get; set; } = null!;

    public string estatus { get; set; } = null!;

    public virtual ICollection<ComprobanteFiscal> ComprobantesFiscales { get; set; } = new List<ComprobanteFiscal>();

    // Métodos de dominio
    public void SetRncCedula(string rncCedula)
    {
        var rncCedulaValue = new RncCedula(rncCedula);
        this.rncCedula = rncCedulaValue.Value;
    }

    public bool IsActivo() => estatus?.ToLower() == "activo";

    public bool IsPersonaFisica() => tipo?.ToLower() == "persona fisica";

    public bool IsPersonaJuridica() => tipo?.ToLower() == "persona juridica";
}
