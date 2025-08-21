using DGII.Domain.Entities;

namespace DGII.Application.Interfaces.Persistence;

public interface IContribuyenteRepository : IRepository<Contribuyente>
{
    /// <summary>
    /// Obtiene un contribuyente por su RNC/Cédula
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula del contribuyente</param>
    /// <returns>El contribuyente encontrado o null si no existe</returns>
    Task<Contribuyente?> GetByRncCedulaAsync(string rncCedula);

    /// <summary>
    /// Verifica si existe un contribuyente con el RNC/Cédula especificado
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    Task<bool> ExistsByRncCedulaAsync(string rncCedula);

    /// <summary>
    /// Obtiene todos los contribuyentes activos
    /// </summary>
    /// <returns>Lista de contribuyentes activos</returns>
    Task<IList<Contribuyente>> GetActivosAsync();

    /// <summary>
    /// Obtiene contribuyentes por tipo
    /// </summary>
    /// <param name="tipo">El tipo de contribuyente (PERSONA FISICA o PERSONA JURIDICA)</param>
    /// <returns>Lista de contribuyentes del tipo especificado</returns>
    Task<IList<Contribuyente>> GetByTipoAsync(string tipo);
}
