using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DGII.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DgiiDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DgiiDbContext>>();

        try
        {
            logger.LogInformation("Iniciando inicialización de la base de datos...");

            // Verificar si la base de datos existe y tiene las tablas
            if (!await context.Database.CanConnectAsync())
            {
                logger.LogInformation("Creando base de datos...");
                await context.Database.EnsureCreatedAsync();
            }
            else
            {
                logger.LogInformation("Base de datos ya existe, verificando migraciones...");
                try
                {
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migraciones aplicadas exitosamente");
                }
                catch (Exception ex)
                {
                    logger.LogWarning("Error al aplicar migraciones: {Message}. Continuando con seed data...", ex.Message);
                }
            }

            // Ejecutar seed data
            await context.SeedDataAsync();
            logger.LogInformation("Seed data ejecutado exitosamente");

            logger.LogInformation("Inicialización de la base de datos completada");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error durante la inicialización de la base de datos");
            throw;
        }
    }
}
