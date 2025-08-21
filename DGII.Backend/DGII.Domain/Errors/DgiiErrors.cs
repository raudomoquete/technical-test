using ErrorOr;

namespace DGII.Domain.Errors;

public static class DgiiErrors
{
    // Contribuyente Errors
    public static class Contribuyente
    {
        public static Error NotFound(string rncCedula) =>
            Error.NotFound(
                code: "Contribuyente.NotFound",
                description: $"No se encontró el contribuyente con RNC/Cédula {rncCedula}");

        public static Error DuplicateRncCedula(string rncCedula) =>
            Error.Conflict(
                code: "Contribuyente.DuplicateRncCedula",
                description: $"Ya existe un contribuyente con el RNC/Cédula {rncCedula}");

        public static Error InvalidRncCedula(string rncCedula) =>
            Error.Validation(
                code: "Contribuyente.InvalidRncCedula",
                description: $"El RNC/Cédula {rncCedula} no tiene un formato válido");

        public static Error InactiveContribuyente(string rncCedula) =>
            Error.Failure(
                code: "Contribuyente.Inactive",
                description: $"El contribuyente con RNC/Cédula {rncCedula} está inactivo");
    }

    // ComprobanteFiscal Errors
    public static class ComprobanteFiscal
    {
        public static Error NotFound(string ncf) =>
            Error.NotFound(
                code: "ComprobanteFiscal.NotFound",
                description: $"No se encontró el comprobante fiscal con NCF {ncf}");

        public static Error DuplicateNcf(string ncf) =>
            Error.Conflict(
                code: "ComprobanteFiscal.DuplicateNCF",
                description: $"Ya existe un comprobante fiscal con el NCF {ncf}");

        public static Error InvalidNcf(string ncf) =>
            Error.Validation(
                code: "ComprobanteFiscal.InvalidNCF",
                description: $"El NCF {ncf} no tiene un formato válido");

        public static Error InvalidAmount(decimal amount) =>
            Error.Validation(
                code: "ComprobanteFiscal.InvalidAmount",
                description: $"El monto {amount} no es válido");

        public static Error InvalidItbis(decimal itbis) =>
            Error.Validation(
                code: "ComprobanteFiscal.InvalidItbis",
                description: $"El ITBIS {itbis} no es válido");
    }

    // General Errors
    public static class General
    {
        public static Error DatabaseConnection =>
            Error.Failure(
                code: "General.DatabaseConnection",
                description: "Error de conexión con la base de datos");

        public static Error Unauthorized =>
            Error.Unauthorized(
                code: "General.Unauthorized",
                description: "No autorizado para realizar esta operación");

        public static Error Validation(string field, string message) =>
            Error.Validation(
                code: $"General.Validation.{field}",
                description: message);
    }
}
