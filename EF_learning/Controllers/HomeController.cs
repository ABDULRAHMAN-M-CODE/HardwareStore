using Intro;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EF_learning.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult AddData()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        

    }
}
