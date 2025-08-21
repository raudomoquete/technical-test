namespace DGII.Domain.ValueObjects;

public record Monto
{
    public decimal Value { get; }

    public Monto(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El monto no puede ser negativo.", nameof(value));

        if (value > 999999999.99m)
            throw new ArgumentException("El monto excede el límite máximo permitido.", nameof(value));

        Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    public static Monto Zero => new(0m);

    public static bool IsValid(decimal value)
    {
        return value >= 0 && value <= 999999999.99m;
    }

    public static implicit operator decimal(Monto monto) => monto.Value;

    public static explicit operator Monto(decimal value) => new(value);

    public static Monto operator +(Monto a, Monto b) => new(a.Value + b.Value);

    public static Monto operator -(Monto a, Monto b) => new(a.Value - b.Value);

    public static Monto operator *(Monto a, decimal factor) => new(a.Value * factor);

    public override string ToString() => Value.ToString("F2");
}
