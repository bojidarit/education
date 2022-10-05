using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Models;
using SportsStore.Controllers;
using Xunit;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        // Arrange
        var products = new Product[]{
            new Product{ Id = 1, Name = "P1" },
            new Product{ Id = 2, Name = "P2" },
        };
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products)
            .Returns(products.AsQueryable<Product>());

        var controller = new HomeController(mock.Object);

        // Act
        var result = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;

        // Assert
        var productsArray = result?.ToArray() ?? Array.Empty<Product>();
        Assert.True(productsArray.Length == 2);
        Assert.Equal("P1", productsArray[0].Name);
        Assert.Equal("P2", productsArray[1].Name);
    }
}