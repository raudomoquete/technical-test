using DGII.Domain.Entities;
using DGII.Domain.Enums;
using DGII.Infrastructure.Data;
using DGII.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DGII.Tests.Infrastructure.Repositories;

public class ContribuyenteRepositoryTests : IDisposable
{
    private readonly DgiiDbContext _context;
    private readonly ContribuyenteRepository _repository;

    public ContribuyenteRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DgiiDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DgiiDbContext(options);
        _repository = new ContribuyenteRepository(_context);
    }

    [Fact]
    public async Task GetByRncCedulaAsync_WithExistingContribuyente_ShouldReturnContribuyente()
    {
        // Arrange
        var contribuyente = new Contribuyente
        {
            Id = Guid.NewGuid(),
            rncCedula = "98754321012",
            nombre = "JUAN PEREZ",
            tipo = TipoContribuyente.PersonaFisica.ToDisplayString(),
            estatus = EstatusContribuyente.Activo.ToDisplayString()
        };

        await _context.Contribuyentes.AddAsync(contribuyente);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByRncCedulaAsync("98754321012");

        // Assert
        result.Should().NotBeNull();
        result!.rncCedula.Should().Be("98754321012");
        result.nombre.Should().Be("JUAN PEREZ");
    }

    [Fact]
    public async Task GetByRncCedulaAsync_WithNonExistingContribuyente_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByRncCedulaAsync("999999999");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByRncCedulaAsync_WithExistingContribuyente_ShouldReturnTrue()
    {
        // Arrange
        var contribuyente = new Contribuyente
        {
            Id = Guid.NewGuid(),
            rncCedula = "98754321012",
            nombre = "JUAN PEREZ",
            tipo = TipoContribuyente.PersonaFisica.ToDisplayString(),
            estatus = EstatusContribuyente.Activo.ToDisplayString()
        };

        await _context.Contribuyentes.AddAsync(contribuyente);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsByRncCedulaAsync("98754321012");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByRncCedulaAsync_WithNonExistingContribuyente_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsByRncCedulaAsync("999999999");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetActivosAsync_ShouldReturnOnlyActiveContribuyentes()
    {
        // Arrange
        var contribuyentes = new List<Contribuyente>
        {
            new()
            {
                Id = Guid.NewGuid(),
                rncCedula = "98754321012",
                nombre = "JUAN PEREZ",
                tipo = TipoContribuyente.PersonaFisica.ToDisplayString(),
                estatus = EstatusContribuyente.Activo.ToDisplayString()
            },
            new()
            {
                Id = Guid.NewGuid(),
                rncCedula = "123456789",
                nombre = "FARMACIA TU SALUD",
                tipo = TipoContribuyente.PersonaJuridica.ToDisplayString(),
                estatus = EstatusContribuyente.Inactivo.ToDisplayString()
            }
        };

        await _context.Contribuyentes.AddRangeAsync(contribuyentes);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActivosAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().estatus.Should().Be("activo");
    }

    [Fact]
    public async Task GetByTipoAsync_ShouldReturnContribuyentesByType()
    {
        // Arrange
        var contribuyentes = new List<Contribuyente>
        {
            new()
            {
                Id = Guid.NewGuid(),
                rncCedula = "98754321012",
                nombre = "JUAN PEREZ",
                tipo = TipoContribuyente.PersonaFisica.ToDisplayString(),
                estatus = EstatusContribuyente.Activo.ToDisplayString()
            },
            new()
            {
                Id = Guid.NewGuid(),
                rncCedula = "123456789",
                nombre = "FARMACIA TU SALUD",
                tipo = TipoContribuyente.PersonaJuridica.ToDisplayString(),
                estatus = EstatusContribuyente.Inactivo.ToDisplayString()
            }
        };

        await _context.Contribuyentes.AddRangeAsync(contribuyentes);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByTipoAsync(TipoContribuyente.PersonaFisica.ToDisplayString());

        // Assert
        result.Should().HaveCount(1);
        result.First().tipo.Should().Be("PERSONA FISICA");
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
