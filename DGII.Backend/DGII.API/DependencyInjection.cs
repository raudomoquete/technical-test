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
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }
            );
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
