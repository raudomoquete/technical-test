namespace DGII.Domain.Enums;

public enum TipoContribuyente
{
    PersonaFisica,
    PersonaJuridica
}

public static class TipoContribuyenteExtensions
{
    public static string ToDisplayString(this TipoContribuyente tipo)
    {
        return tipo switch
        {
            TipoContribuyente.PersonaFisica => "PERSONA FISICA",
            TipoContribuyente.PersonaJuridica => "PERSONA JURIDICA",
            _ => throw new ArgumentException("Tipo de contribuyente no válido")
        };
    }

    public static TipoContribuyente FromDisplayString(string displayString)
    {
        return displayString?.ToUpper() switch
        {
            "PERSONA FISICA" => TipoContribuyente.PersonaFisica,
            "PERSONA JURIDICA" => TipoContribuyente.PersonaJuridica,
            _ => throw new ArgumentException($"Tipo de contribuyente no válido: {displayString}")
        };
    }
}
