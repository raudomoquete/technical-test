using DGII.Application.Features.Contribuyentes.Queries.GetAllContribuyentes;
using DGII.Application.Interfaces.Persistence;
using DGII.Domain.Entities;
using DGII.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using DGII.Tests.Common;

namespace DGII.Tests.Application.Features.Contribuyentes.Queries;

public class GetAllContribuyentesQueryHandlerTests : TestBase
{
    private readonly Mock<IContribuyenteRepository> _mockRepository;
    private readonly Mock<ILogger<GetAllContribuyentesQueryHandler>> _mockLogger;
    private readonly GetAllContribuyentesQueryHandler _handler;

    public GetAllContribuyentesQueryHandlerTests()
    {
        _mockRepository = new Mock<IContribuyenteRepository>();
        _mockLogger = new Mock<ILogger<GetAllContribuyentesQueryHandler>>();
        _handler = new GetAllContribuyentesQueryHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnContribuyentes()
    {
        // Arrange
        var query = new GetAllContribuyentesQuery();
        var expectedContribuyentes = new List<Contribuyente>
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

        _mockRepository.Setup(x => x.GetAllAsync(null, null, null, true, false))
            .ReturnsAsync(expectedContribuyentes);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
        
        var contribuyentes = result.Value.ToList();
        contribuyentes[0].rncCedula.Should().Be("98754321012");
        contribuyentes[0].nombre.Should().Be("JUAN PEREZ");
        contribuyentes[0].tipo.Should().Be("PERSONA FISICA");
        contribuyentes[0].estatus.Should().Be("activo");
        
        contribuyentes[1].rncCedula.Should().Be("123456789");
        contribuyentes[1].nombre.Should().Be("FARMACIA TU SALUD");
        contribuyentes[1].tipo.Should().Be("PERSONA JURIDICA");
        contribuyentes[1].estatus.Should().Be("inactivo");

        _mockRepository.Verify(x => x.GetAllAsync(null, null, null, true, false), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyRepository_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllContribuyentesQuery();
        _mockRepository.Setup(x => x.GetAllAsync(null, null, null, true, false))
            .ReturnsAsync(new List<Contribuyente>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();

        _mockRepository.Verify(x => x.GetAllAsync(null, null, null, true, false), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnError()
    {
        // Arrange
        var query = new GetAllContribuyentesQuery();
        var exception = new Exception("Database connection failed");
        
        _mockRepository.Setup(x => x.GetAllAsync(null, null, null, true, false))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].Code.Should().Be("Contribuyentes.GetAll");

        _mockRepository.Verify(x => x.GetAllAsync(null, null, null, true, false), Times.Once);
    }
}
