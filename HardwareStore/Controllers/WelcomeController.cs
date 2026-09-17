using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
