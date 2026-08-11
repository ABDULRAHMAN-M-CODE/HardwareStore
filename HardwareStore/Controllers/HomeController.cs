using HardwareStoreNameSpace;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HardwareStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
