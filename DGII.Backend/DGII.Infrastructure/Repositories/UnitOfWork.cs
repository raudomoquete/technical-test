using DGII.Application.Interfaces;
using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Transactions;

namespace DGII.Infrastructure.Repositories;

public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork
        where TContext : DbContext
{
    private readonly TContext _context;
    private bool disposed = false;
    private Dictionary<Type, object> repositories;

    // Repositorios específicos
    private IContribuyenteRepository? _contribuyenteRepository;
    private IComprobanteFiscalRepository? _comprobanteFiscalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets the db context.
    /// </summary>
    /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
    public TContext DbContext => _context;

    /// <summary>
    /// Gets the contribuyente repository.
    /// </summary>
    public IContribuyenteRepository ContribuyenteRepository
    {
        get
        {
            _contribuyenteRepository ??= new ContribuyenteRepository(_context);
            return _contribuyenteRepository;
        }
    }

    /// <summary>
    /// Gets the comprobante fiscal repository.
    /// </summary>
    public IComprobanteFiscalRepository ComprobanteFiscalRepository
    {
        get
        {
            _comprobanteFiscalRepository ??= new ComprobanteFiscalRepository(_context);
            return _comprobanteFiscalRepository;
        }
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">The disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // clear repositories
                if (repositories != null)
                {
                    repositories.Clear();
                }

                // dispose the db context.
                _context.Dispose();
            }
        }

        disposed = true;
    }

    public int ExecuteSqlCommand(string sql, params object[] parameters)
    {
        return _context.Database.ExecuteSqlRaw(sql, parameters);
    }

    public IQueryable<TEntity> FromRawSql<TEntity>(string sql, params object[] parameters)
        where TEntity : class
    {
        return _context.Set<TEntity>().FromSqlRaw(sql, parameters);
    }

    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false)
        where TEntity : class
    {
        if (repositories == null)
        {
            repositories = new Dictionary<Type, object>();
        }

        // what's the best way to support custom reposity?
        if (hasCustomRepository)
        {
            var customRepo = _context.GetService<IRepository<TEntity>>();
            if (customRepo != null)
            {
                return customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!repositories.ContainsKey(type))
        {
            repositories[type] = new BaseRepository<TEntity>(_context);
        }

        return (IRepository<TEntity>)repositories[type];
    }

    public Task RollBack()
    {
        _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks)
    {
        using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var count = 0;
            foreach (var unitOfWork in unitOfWorks)
            {
                count += await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }

            count += await SaveChangesAsync();

            ts.Complete();

            return count;
        }
    }

    #region Métodos específicos para DGII

    /// <summary>
    /// Obtiene un contribuyente por su RNC/Cédula
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula del contribuyente</param>
    /// <returns>El contribuyente encontrado o null si no existe</returns>
    public async Task<Contribuyente?> GetContribuyenteByRncCedulaAsync(string rncCedula)
    {
        return await ContribuyenteRepository.GetByRncCedulaAsync(rncCedula);
    }

    /// <summary>
    /// Obtiene todos los comprobantes fiscales de un contribuyente por RNC/Cédula
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula del contribuyente</param>
    /// <returns>Lista de comprobantes fiscales del contribuyente</returns>
    public async Task<IList<ComprobanteFiscal>> GetComprobantesByContribuyenteAsync(string rncCedula)
    {
        return await ComprobanteFiscalRepository.GetByContribuyenteRncCedulaAsync(rncCedula);
    }

    /// <summary>
    /// Calcula el total de ITBIS de todos los comprobantes fiscales de un contribuyente
    /// </summary>
    /// <param name="rncCedula">El RNC/Cédula del contribuyente</param>
    /// <returns>El total de ITBIS</returns>
    public async Task<decimal> GetTotalItbisByContribuyenteAsync(string rncCedula)
    {
        return await ComprobanteFiscalRepository.GetTotalItbisByContribuyenteRncCedulaAsync(rncCedula);
    }

    /// <summary>
    /// Obtiene todos los comprobantes fiscales con información del contribuyente incluida
    /// </summary>
    /// <returns>Lista de comprobantes fiscales con contribuyente</returns>
    public async Task<IList<ComprobanteFiscal>> GetAllComprobantesWithContribuyenteAsync()
    {
        return await ComprobanteFiscalRepository.GetAllWithContribuyenteAsync();
    }

    #endregion
}
