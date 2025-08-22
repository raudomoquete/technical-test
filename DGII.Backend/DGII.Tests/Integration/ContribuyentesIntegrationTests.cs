using DGII.API;
using DGII.Application;
using DGII.Infrastructure;
using DGII.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace DGII.Tests.Integration;

public class ContribuyentesIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContribuyentesIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DgiiDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DgiiDbContext>(options =>
                {
                    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Dgii_test;Integrated Security = true");
                });
            });
        });
    }

    [Fact]
    public async Task GetAllContribuyentes_ShouldReturnOkWithData()
    {
        var client = _factory.CreateClient();
        await SeedTestData(_factory.Services);

        var response = await client.GetAsync("/api/contribuyentes");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var contribuyentes = JsonSerializer.Deserialize<List<object>>(content);
        contribuyentes.Should().NotBeNull();
        contribuyentes!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetComprobantesByContribuyente_WithValidRncCedula_ShouldReturnOk()
    {
        var client = _factory.CreateClient();
        var rncCedula = "98754321012";
        await SeedTestData(_factory.Services);

        var response = await client.GetAsync($"/api/contribuyentes/{rncCedula}/comprobantes");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var comprobantes = JsonSerializer.Deserialize<List<object>>(content);
        comprobantes.Should().NotBeNull();
        comprobantes!.Count.Should().BeGreaterThan(0);
    }

    private async Task SeedTestData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DgiiDbContext>();

        context.Contribuyentes.RemoveRange(context.Contribuyentes);
        context.ComprobantesFiscales.RemoveRange(context.ComprobantesFiscales);
        await context.SaveChangesAsync();

        var contribuyente = new DGII.Domain.Entities.Contribuyente
        {
            Id = Guid.NewGuid(),
            rncCedula = "98754321012",
            nombre = "JUAN PEREZ",
            tipo = "PERSONA FISICA",
            estatus = "activo"
        };

        await context.Contribuyentes.AddAsync(contribuyente);
        await context.SaveChangesAsync();

        var comprobantes = new List<DGII.Domain.Entities.ComprobanteFiscal>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ContribuyenteId = contribuyente.Id,
                NCF = "E310000000001",
                monto = 200.00m,
                itbis18 = 36.00m
            },
            new()
            {
                Id = Guid.NewGuid(),
                ContribuyenteId = contribuyente.Id,
                NCF = "E310000000002",
                monto = 1000.00m,
                itbis18 = 180.00m
            }
        };

        await context.ComprobantesFiscales.AddRangeAsync(comprobantes);
        await context.SaveChangesAsync();
    }
}
