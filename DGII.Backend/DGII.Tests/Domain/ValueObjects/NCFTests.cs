using DGII.Domain.ValueObjects;

namespace DGII.Tests.Domain.ValueObjects;

public class NCFTests
{
    [Theory]
    [InlineData("E310000000001")] // Ajustar a 11 dígitos
    [InlineData("E310000000002")] // Ajustar a 11 dígitos
    [InlineData("E310000000003")] // Ajustar a 11 dígitos
    public void Constructor_WithValidNCF_ShouldCreateInstance(string validNCF)
    {
        // Act
        var ncf = new NCF(validNCF);

        // Assert
        ncf.Value.Should().Be(validNCF);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithNullOrEmpty_ShouldThrowArgumentException(string invalidNCF)
    {
        // Act & Assert
        var action = () => new NCF(invalidNCF!);
        action.Should().Throw<ArgumentException>()
            .WithMessage("*NCF no puede estar vacío*");
    }

    [Theory]
    [InlineData("E3100000000")] // Muy corto
    [InlineData("E3100000000000")] // Muy largo
    [InlineData("A310000000001")] // No empieza con E
    [InlineData("E31000000000A")] // Contiene letras en lugar de números
    [InlineData("E-31000000001")] // Contiene caracteres especiales
    public void Constructor_WithInvalidFormat_ShouldThrowArgumentException(string invalidNCF)
    {
        // Act & Assert
        var action = () => new NCF(invalidNCF);
        action.Should().Throw<ArgumentException>()
            .WithMessage("*formato del NCF no es válido*");
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        var ncf = new NCF("E310000000001");

        // Act
        string result = ncf;

        // Assert
        result.Should().Be("E310000000001");
    }

    [Fact]
    public void ExplicitOperator_ShouldConvertFromString()
    {
        // Arrange
        string ncfString = "E310000000001";

        // Act
        var ncf = (NCF)ncfString;

        // Assert
        ncf.Value.Should().Be("E310000000001");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var ncf1 = new NCF("E310000000001");
        var ncf2 = new NCF("E310000000001");

        // Act & Assert
        ncf1.Should().Be(ncf2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var ncf1 = new NCF("E310000000001");
        var ncf2 = new NCF("E310000000002");

        // Act & Assert
        ncf1.Should().NotBe(ncf2);
    }
}
