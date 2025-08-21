using ErrorOr;
using System.Text.Json;

namespace DGII.API.Middleware;

public class ErrorOrMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorOrMiddleware> _logger;

    public ErrorOrMiddleware(RequestDelegate next, ILogger<ErrorOrMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        memoryStream.Position = 0;
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

        // Solo procesar si la respuesta es JSON y contiene ErrorOr
        if (context.Response.ContentType?.Contains("application/json") == true && 
            !string.IsNullOrEmpty(responseBody))
        {
            try
            {
                // Intentar deserializar como ErrorOr
                var errorOrResponse = JsonSerializer.Deserialize<ErrorOrResponse>(responseBody);
                
                if (errorOrResponse?.Errors != null && errorOrResponse.Errors.Any())
                {
                    _logger.LogWarning("ErrorOr response detected with {ErrorCount} errors", errorOrResponse.Errors.Count);
                    
                    // Log cada error para debugging
                    foreach (var error in errorOrResponse.Errors)
                    {
                        _logger.LogWarning("Error: {Code} - {Description}", error.Code, error.Description);
                    }
                }
            }
            catch (JsonException)
            {
                // No es un ErrorOr, continuar normalmente
            }
        }

        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(originalBodyStream);
    }

    private class ErrorOrResponse
    {
        public List<ErrorInfo>? Errors { get; set; }
    }

    private class ErrorInfo
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }
}
