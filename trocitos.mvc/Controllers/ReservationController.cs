using Microsoft.AspNetCore.Mvc;
using trocitos.mvc.Services;
using trocitos.mvc.Models;

namespace trocitos.mvc.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
