using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using Gbook.Models;
using Gbook.Repository; 
using System.Collections.Generic;
using System.Threading.Tasks;
using Gbook.Models;
using Microsoft.EntityFrameworkCore;

namespace Gbook.Controllers
{


    public class HomeController : Controller
    {
        IRepository repo; 

        public HomeController(IRepository r)
        {
            repo = r;
        }

        public async Task<IActionResult> Index()
        {
            var model = await repo.GetAllMessagesAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }

}
