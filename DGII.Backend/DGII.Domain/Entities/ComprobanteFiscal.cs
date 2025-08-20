using DGII.Infrastructure;

namespace DGII.Domain.Entities;

public class ComprobanteFiscal
{
    public Guid Id { get; set; }

    public Guid ContribuyenteId { get; set; }

    public string NCF { get; set; } = null!;

    public decimal monto { get; set; }

    public decimal itbis18 { get; set; }

    public virtual Contribuyente Contribuyente { get; set; } = null!;
}
