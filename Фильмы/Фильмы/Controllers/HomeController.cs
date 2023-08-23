using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Models;
using System.Diagnostics;

namespace UploadFile.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml", _context.Movies.ToList());
        }

        public IActionResult GetMovie(int id)
        {
            
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound(); 
            }

            return View(movie); 
        }
    }
}