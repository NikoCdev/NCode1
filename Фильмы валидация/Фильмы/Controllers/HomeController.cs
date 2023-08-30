using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFile.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;


        public HomeController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml", _context.Movies.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Director,Genre,ReleaseYear,PosterUrl,Description")]  MovieModel _movie, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {          
                    string path = "/uploads/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    _movie.PosterUrl = path;    
            }
            if(ModelState.IsValid)
            {
                _context.Add(_movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(_movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieModel _movie , IFormFile? uploadedFile)
        {
            if (uploadedFile != null)
            {
                
                    string path = "/uploads/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    _movie.PosterUrl = path;                   
                if (ModelState.IsValid)
                {
                    _context.Update(_movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (uploadedFile == null)
            {
                if (ModelState.IsValid)
                {
                    var existingMovie = await _context.Movies.FindAsync(_movie.Id);
                    if (existingMovie == null)
                    {
                        return NotFound();
                    }
                    _movie.PosterUrl = existingMovie.PosterUrl;
                    _context.Entry(existingMovie).CurrentValues.SetValues(_movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                
            }
            return View(_movie);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var student = await _context.Movies.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var student = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

 

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'ApplicationContext.Movies'  is null.");
            }
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                if (movie == null)
                {
                    return NotFound();
                }
       
                var imagePath = Path.Combine(_appEnvironment.WebRootPath, movie.PosterUrl.TrimStart('/'));         
                if (System.IO.File.Exists(imagePath))
                {
                    
                    System.IO.File.Delete(imagePath);
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Problem($"Error: {ex.Message}");
            }
        }


        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}