using DGII.Domain.Entities;

namespace DGII.Application.Interfaces.Persistence;

public interface IComprobanteFiscalRepository : IRepository<ComprobanteFiscal>
{
    /// <summary>
    /// Obtiene un comprobante fiscal por su NCF
    /// </summary>
    /// <param name="ncf">El NCF del comprobante fiscal</param>
    /// <returns>El comprobante fiscal encontrado o null si no existe</returns>
    Task<ComprobanteFiscal?> GetByNcfAsync(string ncf);

    /// <summary>
    /// Verifica si existe un comprobante fiscal con el NCF especificado
    /// </summary>
    /// <param name="ncf">El NCF a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    Task<bool> ExistsByNcfAsync(string ncf);

    /// <summary>
    /// Obtiene todos los comprobantes fiscales de un contribuyente específico
    /// </summary>
    /// <param name="contribuyenteId">El ID del contribuyente</param>
    /// <returns>Lista de comprobantes fiscales del contribuyente</returns>
    Task<IList<ComprobanteFiscal>> GetByContribuyenteIdAsync(Guid contribuyenteId);

    /// <summary>
    /// Obtiene todos los comprobantes fiscales de un contribuyente por RNC/Cédula
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula del contribuyente</param>
    /// <returns>Lista de comprobantes fiscales del contribuyente</returns>
    Task<IList<ComprobanteFiscal>> GetByContribuyenteRncCedulaAsync(string rncCedula);

    /// <summary>
    /// Calcula el total de ITBIS de todos los comprobantes fiscales de un contribuyente
    /// </summary>
    /// <param name="contribuyenteId">El ID del contribuyente</param>
    /// <returns>El total de ITBIS</returns>
    Task<decimal> GetTotalItbisByContribuyenteIdAsync(Guid contribuyenteId);

    /// <summary>
    /// Calcula el total de ITBIS de todos los comprobantes fiscales de un contribuyente por RNC/Cédula
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula del contribuyente</param>
    /// <returns>El total de ITBIS</returns>
    Task<decimal> GetTotalItbisByContribuyenteRncCedulaAsync(string rncCedula);

    /// <summary>
    /// Obtiene todos los comprobantes fiscales con información del contribuyente incluida
    /// </summary>
    /// <returns>Lista de comprobantes fiscales con contribuyente</returns>
    Task<IList<ComprobanteFiscal>> GetAllWithContribuyenteAsync();
}
