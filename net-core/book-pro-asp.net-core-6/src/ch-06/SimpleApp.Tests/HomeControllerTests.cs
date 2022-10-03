using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleApp.Controllers;

namespace SimpleApp.Tests;

public class HomeControllerTests
{
    class FakeDataSource : IDataSource
    {
        public IEnumerable<Product> Products { get; set; }

        public FakeDataSource(Product[] data) =>
            Products = data;
    }

    [Fact]
    public void IndexActionModelIsCompleted()
    {
        // Arrange
        var products = new Product[]
        {
            new("P1", 123.1M),
            new("P2", 456.2M),
            new("P3", 789.3M),
        };
        var data = new FakeDataSource(products);

        var controller = new HomeController();
        controller.dataSource = data;

        //Act
        var model = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;

        // Assert
        Assert.Equal(data.Products, model,
            Comparer.Get<Product>((p1, p2) => p1?.Name == p2?.Name && p1?.Price == p2?.Price));
    }
}
