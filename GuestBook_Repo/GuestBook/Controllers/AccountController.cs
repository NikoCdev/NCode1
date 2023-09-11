
using Microsoft.AspNetCore.Mvc;
using Gbook.Models;
using Gbook.Repository;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gbook.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _repository;

        public AccountController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.GetUserByLoginAsync(logon.Login);

                if (user != null)
                {
                    string salt = user.Salt;

                    byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);
                    var md5 = MD5.Create();
                    byte[] byteHash = md5.ComputeHash(password);

                    StringBuilder hash = new StringBuilder(byteHash.Length);
                    for (int i = 0; i < byteHash.Length; i++)
                        hash.Append(string.Format("{0:X2}", byteHash[i]));

                    if (user.Pwd == hash.ToString())
                    {
                        HttpContext.Session.SetString("Login", user.Login);
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Wrong login or password!");
            }

            return View(logon);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _repository.GetUserByLoginAsync(reg.Login);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User with this login already exists.");
                    return View(reg);
                }

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

                await _repository.AddUserAsync(user);
                return RedirectToAction("Login");
            }

            return View(reg);
        }

        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string messageText)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(messageText))
                {
                    var userLogin = HttpContext.Session.GetString("Login");
                    var user = await _repository.GetUserByLoginAsync(userLogin);

                    if (user != null)
                    {
                        var message = new Message
                        {
                            User = user,
                            MessageText = messageText,
                            MessageDate = DateTime.Now
                        };

                        await _repository.AddMessageAsync(message);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
