namespace SimpleApp.Controllers;

public class HomeController : Controller
{
    public IDataSource dataSource = new ProductDataSource();

    public ViewResult Index() => View(dataSource.Products);
}