using DGII.API.Controllers;
using DGII.Application.Features.Contribuyentes.Queries.GetAllContribuyentes;
using DGII.Application.Features.ComprobantesFiscales.Queries.GetComprobantesFiscalesByContribuyente;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;

namespace DGII.Tests.API.Controllers;

public class ContribuyentesControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger> _mockLogger;
    private readonly ContribuyentesController _controller;

    public ContribuyentesControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger>();
        _controller = new ContribuyentesController(_mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllContribuyentes_WithValidRequest_ShouldReturnOkResult()
    {
        var expectedContribuyentes = new List<ContribuyenteDto>
        {
            new("98754321012", "JUAN PEREZ", "PERSONA FISICA", "activo"),
            new("123456789", "FARMACIA TU SALUD", "PERSONA JURIDICA", "inactivo")
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<GetAllContribuyentesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedContribuyentes);

        var actionResult = await _controller.GetAllContribuyentes();

        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedContribuyentes);
    }

    [Fact]
    public async Task GetComprobantesByContribuyente_WithValidRncCedula_ShouldReturnOkResult()
    {
        var rncCedula = "98754321012";
        var expectedComprobantes = new List<ComprobanteFiscalByContribuyenteDto>
        {
            new(rncCedula, "E310000000001", "200.00", "36.00"),
            new(rncCedula, "E310000000002", "1000.00", "180.00")
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<GetComprobantesFiscalesByContribuyenteQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedComprobantes);

        var actionResult = await _controller.GetComprobantesByContribuyente(rncCedula);

        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedComprobantes);
    }
}
