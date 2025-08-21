using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DGII.Infrastructure.Repositories;

public class ComprobanteFiscalRepository : BaseRepository<ComprobanteFiscal>, IComprobanteFiscalRepository
{
    public ComprobanteFiscalRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ComprobanteFiscal?> GetByNcfAsync(string ncf)
    {
        return await _dbSet
            .FirstOrDefaultAsync(cf => cf.NCF == ncf);
    }

    public async Task<bool> ExistsByNcfAsync(string ncf)
    {
        return await _dbSet
            .AnyAsync(cf => cf.NCF == ncf);
    }

    public async Task<IList<ComprobanteFiscal>> GetByContribuyenteIdAsync(Guid contribuyenteId)
    {
        return await _dbSet
            .Where(cf => cf.ContribuyenteId == contribuyenteId)
            .ToListAsync();
    }

    public async Task<IList<ComprobanteFiscal>> GetByContribuyenteRncCedulaAsync(string rncCedula)
    {
        return await _dbSet
            .Include(cf => cf.Contribuyente)
            .Where(cf => cf.Contribuyente.rncCedula == rncCedula)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalItbisByContribuyenteIdAsync(Guid contribuyenteId)
    {
        return await _dbSet
            .Where(cf => cf.ContribuyenteId == contribuyenteId)
            .SumAsync(cf => cf.itbis18);
    }

    public async Task<decimal> GetTotalItbisByContribuyenteRncCedulaAsync(string rncCedula)
    {
        return await _dbSet
            .Include(cf => cf.Contribuyente)
            .Where(cf => cf.Contribuyente.rncCedula == rncCedula)
            .SumAsync(cf => cf.itbis18);
    }

    public async Task<IList<ComprobanteFiscal>> GetAllWithContribuyenteAsync()
    {
        return await _dbSet
            .Include(cf => cf.Contribuyente)
            .ToListAsync();
    }
}
