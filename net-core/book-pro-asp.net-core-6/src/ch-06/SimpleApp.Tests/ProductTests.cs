namespace SimpleTests.Tests;

public class ProductTests
{
    [Fact]
    public void CanChangeProductName()
    {
        // Arrange
        Product p = new("Test", 100M);
        var checkName = "New Name";

        // Act
        p.Name = checkName;

        // Assert
        Assert.Equal(checkName, p.Name);
    }

    [Fact]
    public void CanChangeProductPrice()
    {
        // Arrange
        Product p = new("Test", 100M);
        var checkPrice = 200M;

        // Act
        p.Price = checkPrice;

        // Assert
        Assert.Equal(checkPrice, p.Price);
    }
}