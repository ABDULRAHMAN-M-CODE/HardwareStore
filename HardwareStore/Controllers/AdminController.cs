using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult ProductsPage()
        {
            return View();
        }
    }
}
