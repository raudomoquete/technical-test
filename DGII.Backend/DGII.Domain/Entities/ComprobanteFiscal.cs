using DGII.Domain.ValueObjects;

namespace DGII.Domain.Entities;

public class ComprobanteFiscal
{
    public Guid Id { get; set; }

    public Guid ContribuyenteId { get; set; }

    public string NCF { get; set; } = null!;

    public decimal monto { get; set; }

    public decimal itbis18 { get; set; }

    public virtual Contribuyente Contribuyente { get; set; } = null!;

    // Métodos de dominio
    public void SetNCF(string ncf)
    {
        var ncfValue = new NCF(ncf);
        NCF = ncfValue.Value;
    }

    public void SetMonto(decimal monto)
    {
        var montoValue = new Monto(monto);
        this.monto = montoValue.Value;
    }

    public void SetItbis(decimal itbis)
    {
        var itbisValue = new Itbis(itbis);
        itbis18 = itbisValue.Value;
    }

    public void CalculateItbis(decimal taxRate = 0.18m)
    {
        var montoValue = new Monto(monto);
        var itbisValue = Itbis.CalculateFromMonto(montoValue, taxRate);
        itbis18 = itbisValue.Value;
    }

    public decimal GetTotalAmount() => monto + itbis18;
}
