namespace PartyInvites.Controllers;

using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;

public class HomeController : Controller
{
    public IActionResult Index() =>
        View();

    [HttpGet]
    public ViewResult RsvpForm() =>
        View();

    [HttpPost]
    public ViewResult RsvpForm(GuestResponse guestResponse)
    {
        if (ModelState.IsValid)
        {
            Repository.AddResponse(guestResponse);
            return View("Thanks", guestResponse);
        }
        else
        {
            return View();
        }
    }

    public ViewResult ListResponses() =>
        View(Repository.Responses.Where(r => r.WillAttend == true));
}
