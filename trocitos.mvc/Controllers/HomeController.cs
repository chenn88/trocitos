using Microsoft.AspNetCore.Mvc;

namespace trocitos.mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}