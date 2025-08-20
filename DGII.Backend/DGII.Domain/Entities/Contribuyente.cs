using DGII.Domain.Entities;

namespace DGII.Infrastructure;

public partial class Contribuyente
{
    public Guid Id { get; set; }

    public string rncCedula { get; set; } = null!;

    public string nombre { get; set; } = null!;

    public string tipo { get; set; } = null!;

    public string estatus { get; set; } = null!;

    public virtual ICollection<ComprobanteFiscal> ComprobantesFiscales { get; set; } = new List<ComprobanteFiscal>();
}
