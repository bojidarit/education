namespace LanguageFeatures.Controllers;


using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public ViewResult Index() =>
        View(new string[] { "C#", "Language", "Features" });
}