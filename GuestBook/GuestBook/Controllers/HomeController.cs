using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gbook.Models;
using System.Linq;

public class HomeController : Controller
{
    private readonly UserContext _context;

    public HomeController(UserContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var messages = _context.Messages
            .Include(m => m.User) 
            .ToList();

        return View(messages);
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }

}

