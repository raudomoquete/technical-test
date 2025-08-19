namespace DGII.Infrastructure.Repositories;

public class GenericRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
{
    public GenericRepository(dbContextDGII dbContext) : base(dbContext)
    {
    }

    //TODO: agregar cualquier funcionalidad adicional para GenericRepository
}
