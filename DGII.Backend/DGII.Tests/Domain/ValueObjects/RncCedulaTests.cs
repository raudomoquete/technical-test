using DGII.Domain.ValueObjects;
using FluentAssertions;

namespace DGII.Tests.Domain.ValueObjects;

public class RncCedulaTests
{
    [Theory]
    [InlineData("123456789")]
    [InlineData("987654321")]
    [InlineData("12345678901")]
    public void Constructor_WithValidRncCedula_ShouldCreateInstance(string validRncCedula)
    {
        // Act
        var rncCedula = new RncCedula(validRncCedula);

        // Assert
        rncCedula.Value.Should().Be(validRncCedula);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithNullOrEmpty_ShouldThrowArgumentException(string invalidRncCedula)
    {
        // Act & Assert
        var action = () => new RncCedula(invalidRncCedula!);
        action.Should().Throw<ArgumentException>()
            .WithMessage("*RNC/Cédula no puede estar vacío*");
    }

    [Theory]
    [InlineData("12345678")] // Muy corto
    [InlineData("123456789012")] // Muy largo
    [InlineData("12345678A")] // Contiene letras
    [InlineData("123-456-789")] // Contiene caracteres especiales
    public void Constructor_WithInvalidFormat_ShouldThrowArgumentException(string invalidRncCedula)
    {
        // Act & Assert
        var action = () => new RncCedula(invalidRncCedula);
        action.Should().Throw<ArgumentException>()
            .WithMessage("*formato del RNC/Cédula no es válido*");
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        var rncCedula = new RncCedula("123456789");

        // Act
        string result = rncCedula;

        // Assert
        result.Should().Be("123456789");
    }

    [Fact]
    public void ExplicitOperator_ShouldConvertFromString()
    {
        // Arrange
        string rncCedulaString = "123456789";

        // Act
        var rncCedula = (RncCedula)rncCedulaString;

        // Assert
        rncCedula.Value.Should().Be("123456789");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var rncCedula1 = new RncCedula("123456789");
        var rncCedula2 = new RncCedula("123456789");

        // Act & Assert
        rncCedula1.Should().Be(rncCedula2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var rncCedula1 = new RncCedula("123456789");
        var rncCedula2 = new RncCedula("987654321");

        // Act & Assert
        rncCedula1.Should().NotBe(rncCedula2);
    }
}
