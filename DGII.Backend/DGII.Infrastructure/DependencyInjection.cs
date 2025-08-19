namespace DGII.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services
            .AddServices()
            .AddPersistence<dbContextDGII>(configuration);

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
       // TO DO: Add the services needed here

        return services;
    }

    private static IServiceCollection AddPersistence<TContext>(
        this IServiceCollection services,
        IConfiguration configuration
    )
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((sp, options) =>
        {

            options.UseSqlServer(configuration.GetConnectionString("db-name"));
        });

        services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
        // Following has a issue: IUnitOfWork cannot support multiple dbcontext/database,
        // that means cannot call AddUnitOfWork<TContext> multiple times.
        // Solution: check IUnitOfWork whether or null
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

        return services;
    }
}
