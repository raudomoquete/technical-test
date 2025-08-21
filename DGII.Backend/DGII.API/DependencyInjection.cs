using System.Text.Json;
using System.Text.Json.Serialization;

namespace DGII.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(
            this IServiceCollection services,
            IConfiguration configuration
        )
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        // Add services to the container.
        services.AddHttpClient();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Configurar serialización JSON según mejores prácticas
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = false; // Para producción
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                
                // Configurar manejo de decimales para montos
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            );

            // Registrar servicios de aplicación
            // services.AddApplication();

        });
        return services;
    }
}
