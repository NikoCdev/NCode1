using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gbook.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

public class HomeController : Controller
{
    private readonly UserContext _context;

    public HomeController(UserContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userList = _context.UsersE.ToList();
        var genresList = _context.Genres.ToList();
        var songsList = _context.Songs.ToList();

        ViewBag.DataLists = new { Genres = genresList, Songs = songsList };

        return View(userList);
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }

    public IActionResult Users()
    {
        var userList = _context.UsersE.ToList();

        return View(userList);
    }

}

