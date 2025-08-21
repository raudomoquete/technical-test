namespace DGII.Domain.Enums;

public enum EstatusContribuyente
{
    Activo,
    Inactivo
}

public static class EstatusContribuyenteExtensions
{
    public static string ToDisplayString(this EstatusContribuyente estatus)
    {
        return estatus switch
        {
            EstatusContribuyente.Activo => "activo",
            EstatusContribuyente.Inactivo => "inactivo",
            _ => throw new ArgumentException("Estatus de contribuyente no válido")
        };
    }

    public static EstatusContribuyente FromDisplayString(string displayString)
    {
        return displayString?.ToLower() switch
        {
            "activo" => EstatusContribuyente.Activo,
            "inactivo" => EstatusContribuyente.Inactivo,
            _ => throw new ArgumentException($"Estatus de contribuyente no válido: {displayString}")
        };
    }
}
