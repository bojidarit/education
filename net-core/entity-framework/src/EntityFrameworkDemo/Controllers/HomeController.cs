using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkDemo.Controllers;

public class HomeController : Controller
{
    public ViewResult Index() =>
        View("Index", "TODO: Some text model data...");
}