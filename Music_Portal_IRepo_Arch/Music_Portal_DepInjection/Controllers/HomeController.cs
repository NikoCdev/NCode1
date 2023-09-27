using Microsoft.AspNetCore.Mvc;
using MusicPortal.Repository;

public class HomeController : Controller
{
    private readonly IRepository _repository;

    public HomeController(IRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var genresList = _repository.GetAllGenresAsync().Result;
        var songsList = _repository.GetAllSongsAsync().Result;

        ViewBag.DataLists = new { Genres = genresList, Songs = songsList };

        return View();
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }

    public async Task<IActionResult> Users()
    {
        var userList = await _repository.GetAllUsersAsync();
        return View(userList);
    }
}