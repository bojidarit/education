using EntityFrameworkDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkDemo.Controllers;

public class HomeController : Controller
{
    private IStoreRepository repository;

    public HomeController(IStoreRepository repo)
    {
        repository = repo;
    }

    public ViewResult Index() =>
        View(repository.Products);
}