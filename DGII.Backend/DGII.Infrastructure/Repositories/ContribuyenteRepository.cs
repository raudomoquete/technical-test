using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DGII.Infrastructure.Repositories;

public class ContribuyenteRepository : BaseRepository<Contribuyente>, IContribuyenteRepository
{
    public ContribuyenteRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Contribuyente?> GetByRncCedulaAsync(string rncCedula)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.rncCedula == rncCedula);
    }

    public async Task<bool> ExistsByRncCedulaAsync(string rncCedula)
    {
        return await _dbSet
            .AnyAsync(c => c.rncCedula == rncCedula);
    }

    public async Task<IList<Contribuyente>> GetActivosAsync()
    {
        return await _dbSet
            .Where(c => c.estatus.ToLower() == "activo")
            .ToListAsync();
    }

    public async Task<IList<Contribuyente>> GetByTipoAsync(string tipo)
    {
        return await _dbSet
            .Where(c => c.tipo.ToUpper() == tipo.ToUpper())
            .ToListAsync();
    }
}
