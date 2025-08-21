namespace DGII.Domain.ValueObjects;

public record Itbis
{
    public decimal Value { get; }

    public Itbis(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El ITBIS no puede ser negativo.", nameof(value));

        if (value > 999999999.99m)
            throw new ArgumentException("El ITBIS excede el límite máximo permitido.", nameof(value));

        Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    public static Itbis Zero => new(0m);

    public static Itbis CalculateFromMonto(Monto monto, decimal taxRate = 0.18m)
    {
        return new Itbis(monto.Value * taxRate);
    }

    public static bool IsValid(decimal value)
    {
        return value >= 0 && value <= 999999999.99m;
    }

    public static implicit operator decimal(Itbis itbis) => itbis.Value;

    public static explicit operator Itbis(decimal value) => new(value);

    public static Itbis operator +(Itbis a, Itbis b) => new(a.Value + b.Value);

    public static Itbis operator -(Itbis a, Itbis b) => new(a.Value - b.Value);

    public override string ToString() => Value.ToString("F2");
}
