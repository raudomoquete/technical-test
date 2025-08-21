using System.Text.RegularExpressions;

namespace DGII.Domain.ValueObjects;

public record NCF
{
    public string Value { get; }

    public NCF(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El NCF no puede estar vacío.", nameof(value));

        if (!IsValidNCF(value))
            throw new ArgumentException("El formato del NCF no es válido.", nameof(value));

        Value = value.Trim().ToUpper();
    }

    public static bool IsValidNCF(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        // NCF debe tener exactamente 19 caracteres
        if (value.Length != 19)
            return false;

        // NCF debe seguir el formato: E + 11 dígitos + 7 dígitos
        // Ejemplo: E310000000001
        var pattern = @"^[A-Z]\d{11}$";
        return Regex.IsMatch(value, pattern);
    }

    public static implicit operator string(NCF ncf) => ncf.Value;

    public static explicit operator NCF(string value) => new(value);

    public override string ToString() => Value;
}
