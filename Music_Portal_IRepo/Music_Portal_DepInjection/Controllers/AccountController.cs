
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Gbook.Models;
using Gbook.Repository;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gbook.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _appEnvironment;

        public AccountController(IRepository repository, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _appEnvironment = appEnvironment;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if (_repository.GetAllUsersAsync().Result.Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }

                var users = _repository.GetUserByLoginAsync(logon.Login).Result;

                if (users == null)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }

                if (!users.IsActivated)
                {
                    ModelState.AddModelError("", "Your account is not activated!");
                    return View(logon);
                }

                string salt = users.Salt;
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);
                var md5 = MD5.Create();
                byte[] byteHash = md5.ComputeHash(password);
                StringBuilder hash = new StringBuilder(byteHash.Length);

                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (users.Pwd != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }

                HttpContext.Session.SetString("Login", users.Login);
                HttpContext.Session.SetInt32("UserId", users.Id);
                HttpContext.Session.SetString("Status", users.Status);

                return RedirectToAction("Index", "Home");
            }
            return View(logon);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Login = reg.Login;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);
                var md5 = MD5.Create();
                byte[] byteHash = md5.ComputeHash(password);
                StringBuilder hash = new StringBuilder(byteHash.Length);

                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Pwd = hash.ToString();
                user.Salt = salt;
                user.Status = "User";
                user.IsActivated = false;

                _repository.AddUserAsync(user).Wait();

                return RedirectToAction("Login");
            }

            return View(reg);
        }

        public IActionResult ApproveRegistration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveRegistration(int userId)
        {
            var user = _repository.GetUserByIdAsync(userId).Result;

            if (user != null)
            {
                user.IsActivated = true;
                _repository.UpdateUserAsync(user).Wait();

                return RedirectToAction("Users", "Home");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            var user = _repository.GetUserByIdAsync(userId).Result;

            if (user != null)
            {
                if (user.Status != "Admin")
                {
                    _repository.DeleteUserAsync(user).Wait();
                }

                return RedirectToAction("Users", "Home");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult UpgradeUser(int userId)
        {
            var user = _repository.GetUserByIdAsync(userId).Result;

            if (user != null)
            {
                if (user.Status != "Admin")
                {
                    user.Status = "Admin";
                    _repository.UpdateUserAsync(user).Wait();
                }

                return RedirectToAction("Users", "Home");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult AddSong()
        {
            var genresList = _repository.GetAllGenresAsync().Result;
            ViewBag.GenresList = new SelectList(genresList, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSong(Song Song, IFormFile uploadedPhoto, IFormFile uploadedAudio)
        {
            if (!ModelState.IsValid)
            {
                var genresList = _repository.GetAllGenresAsync().Result;
                ViewBag.GenresList = new SelectList(genresList, "Id", "Name");
                return View(Song);
            }

            if (Song.GenreId == null)
            {
                ModelState.AddModelError("GenreId", "Пожалуйста, выберите жанр для песни.");

                var genresList = _repository.GetAllGenresAsync().Result;
                ViewBag.GenresList = new SelectList(genresList, "Id", "Name");
                return View(Song);
            }

            if (uploadedAudio != null)
            {
                string AudioPath = "/Audio/" + uploadedAudio.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + AudioPath, FileMode.Create))
                {
                    await uploadedAudio.CopyToAsync(fileStream);
                }
                Song.AudioFile = AudioPath;
            }

            if (uploadedPhoto != null)
            {
                string PhotoPath = "/Images/" + uploadedPhoto.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + PhotoPath, FileMode.Create))
                {
                    await uploadedPhoto.CopyToAsync(fileStream);
                }
                Song.ImageFile = PhotoPath;
            }

            await _repository.AddSongAsync(Song);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var song = await _repository.GetSongByIdAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _repository.GetSongByIdAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            await _repository.DeleteSongAsync(song);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditSong(int id)
        {
            var song = await _repository.GetSongByIdAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            var genresList = await _repository.GetAllGenresAsync();
            ViewBag.GenresList = new SelectList(genresList, "Id", "Name");

            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> EditSong(int id, Song editedSong, IFormFile? uploadedPhoto, IFormFile? uploadedAudio)
        {
            if (id != editedSong.Id)
            {
                return NotFound();
            }

            if (editedSong.GenreId == null)
            {
                ModelState.AddModelError("editedSong.GenreId", "Пожалуйста, выберите жанр для песни.");
            }

            if (ModelState.IsValid)
            {
                var existingSong = await _repository.GetSongByIdAsync(id);

                existingSong.Title = editedSong.Title;
                existingSong.Singer = editedSong.Singer;
                existingSong.GenreId = editedSong.GenreId;

                if (uploadedAudio != null)
                {
                    string AudioPath = "/Audio/" + uploadedAudio.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + AudioPath, FileMode.Create))
                    {
                        await uploadedAudio.CopyToAsync(fileStream);
                    }
                    existingSong.AudioFile = AudioPath;
                }

                if (uploadedPhoto != null)
                {
                    string PhotoPath = "/Images/" + uploadedPhoto.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + PhotoPath, FileMode.Create))
                    {
                        await uploadedPhoto.CopyToAsync(fileStream);
                    }
                    existingSong.ImageFile = PhotoPath;
                }

                await _repository.UpdateSongAsync(existingSong);

                return RedirectToAction("Index", "Home");
            }
            else
            {               
                var genresList = await _repository.GetAllGenresAsync();
                ViewBag.GenresList = new SelectList(genresList, "Id", "Name");                           
                return View(editedSong);
            }
        }


        [HttpGet]
        public IActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(Genre model)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddGenreAsync(model);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


    }
}