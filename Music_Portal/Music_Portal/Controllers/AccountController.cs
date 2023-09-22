using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gbook.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Gbook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserContext _context;
        IWebHostEnvironment _appEnvironment;

        public AccountController(UserContext context , IWebHostEnvironment appEnvironment)
        {
            _context = context;
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
                if (_context.UsersE.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var users = _context.UsersE.Where(a => a.Login == logon.Login);
                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var userCheck = _context.UsersE.SingleOrDefault(a => a.Login == logon.Login);              
                if (userCheck.IsActivated == false)
                {
                    ModelState.AddModelError("", "Your account is not activated!");
                    return View(logon);
                }

                var user = users.First();
                string? salt = user.Salt;

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Pwd != hash.ToString())
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Status", user.Status);
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

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Pwd = hash.ToString();
                user.Salt = salt;
                user.Status = "User";
                user.IsActivated = false;
                _context.UsersE.Add(user);
                _context.SaveChanges();
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


            var user = _context.UsersE.Find(userId);

            if (user != null)
            {
                user.IsActivated = true;
                _context.SaveChanges();
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
            var user = _context.UsersE.Find(userId);

            if (user != null)
            {
                if (user.Status != "Admin")
                {
                    _context.UsersE.Remove(user); // Удаление пользователя
                    _context.SaveChanges(); // Сохранение изменений в базе данных
                }

                return RedirectToAction("Users", "Home"); // Перенаправляем на страницу с пользователями
            }
            else
            {
                return RedirectToAction("Error"); // Перенаправляем на страницу ошибки
            }
        }

        [HttpPost]
        public IActionResult UpgradeUser(int userId)
        {
            var user = _context.UsersE.Find(userId);

            if (user != null)
            {
                if (user.Status != "Admin")
                {
                    user.Status = "Admin"; 
                    _context.SaveChanges(); 
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
            var genresList = _context.Genres.ToList();           
            ViewBag.GenresList = new SelectList(genresList, "Id", "Name");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddSong(Song Song, IFormFile uploadedPhoto, IFormFile uploadedAudio)
        {
            if (!ModelState.IsValid)
            {
                // Если модель не прошла валидацию, возвращаем пользователя на страницу с формой
                // и отображаем ошибки валидации
                var genresList = _context.Genres.ToList();
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

            
            Song.GenreId = Song.GenreId;

            _context.Add(Song);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            //if (id == null || _context.UsersE == null)
            //{
            //    return NotFound();
            //}

            var song = await _context.Songs.FirstOrDefaultAsync(m => m.Id == id);
            //if (Song == null)
            //{
            //    return NotFound();
            //}

            return View(song);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(m => m.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            var genresList = _context.Genres.ToList();
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

            if (ModelState.IsValid)
            {
                var existingSong = await _context.Songs.FindAsync(id);

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

                _context.Update(existingSong);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Если модель недопустима, загрузите список жанров и верните представление с ошибкой
                var genresList = _context.Genres.ToList();
                ViewBag.GenresList = new SelectList(genresList, "Id", "Name");

                return View(editedSong);
            }
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
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
                
                _context.Add(model);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index", "Home");
            }

            
            return View(model);
        }


    }


}