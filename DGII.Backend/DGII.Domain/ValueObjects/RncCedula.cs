using System.Text.RegularExpressions;

namespace DGII.Domain.ValueObjects;

public record RncCedula
{
    public string Value { get; }

    public RncCedula(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El RNC/Cédula no puede estar vacío.", nameof(value));

        if (!IsValidRncCedula(value))
            throw new ArgumentException("El formato del RNC/Cédula no es válido.", nameof(value));

        Value = value.Trim();
    }

    public static bool IsValidRncCedula(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        // Verificar que el valor contenga solo dígitos
        if (!Regex.IsMatch(value, "^\\d{9,11}$"))
            return false;

        return true;
    }

    public static implicit operator string(RncCedula rncCedula) => rncCedula.Value;

    public static explicit operator RncCedula(string value) => new(value);

    public override string ToString() => Value;
}
