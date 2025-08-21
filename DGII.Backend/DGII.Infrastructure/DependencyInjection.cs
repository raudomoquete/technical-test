using DGII.Infrastructure.Data;
using DGII.Infrastructure.Repositories;
using DGII.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            .AddPersistence<DgiiDbContext>(configuration);

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
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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
