using DGII.API;
using DGII.Application;
using DGII.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAPI(builder.Configuration).AddApplication().AddInfrastructure(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "DGII Test",
        Version = "v1",
        Description = "API de prueba para la DGII"
    });
});

builder.Host.UseSerilog(
           (ctx, lc) =>
           {
               lc.ReadFrom.Configuration(ctx.Configuration)
                   .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                   .Enrich.WithProperty(
                       "ApplicationName",
                       typeof(Program).Assembly.GetName().Name
                   )
                   .Enrich.WithProperty("Environment", ctx.HostingEnvironment);
           }
       );

var app = builder.Build();

// To check the Headers
app.UseLogHeadersMiddleware(); // add here right after you create app
app.UseRequestLoggingMiddleware(); // add here right after you create app

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseExceptionHandler(); // Para probar la version .net 8

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
