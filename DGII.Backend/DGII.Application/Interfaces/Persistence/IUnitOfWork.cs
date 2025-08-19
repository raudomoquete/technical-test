namespace DGII.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

    int SaveChanges();
    Task<int> SaveChangesAsync();

    Task RollBack();

    int ExecuteSqlCommand(string sql, params object[] parameters);

    IQueryable<TEntity> FromRawSql<TEntity>(string sql, params object[] parameters)
        where TEntity : class;
}
