using System.Text.RegularExpressions;

namespace DGII.Domain.ValueObjects;

public record NCF
{
    public string Value { get; }

    private static readonly Regex NCFRegex = new Regex(@"^E3\d{11}$", RegexOptions.Compiled);

    public NCF(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("NCF no puede estar vacío");
        }

        if (!NCFRegex.IsMatch(value))
        {
            throw new ArgumentException("El formato del NCF no es válido");
        }

        Value = value;
    }

    public static bool IsValidNCF(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        // NCF debe tener exactamente 19 caracteres
        if (value.Length != 12)
            return false;

        // NCF debe seguir el formato: E + 11 dígitos
        // Ejemplo: E31000000001
        var pattern = @"^E\d{11}$";
        return Regex.IsMatch(value, pattern);
    }

    public static implicit operator string(NCF ncf) => ncf.Value;

    public static explicit operator NCF(string value) => new(value);

    public override string ToString() => Value;
}
