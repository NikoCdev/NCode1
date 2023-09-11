using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Services;
//using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        IMessageSender _sender;
        public HomeController(IMessageSender sender)
        {
            _sender = sender;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2([FromServices] IMessageSender sender)
        {
            return Content(sender.Send());
        }
        public IActionResult Index3()
        {
            var sender = HttpContext.RequestServices.GetService<IMessageSender>();
            return Content(sender.Send());
        }
        public IActionResult Index4()
        {
            return Content(_sender.Send());
        }
    }
}
