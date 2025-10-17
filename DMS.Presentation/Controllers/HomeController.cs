using System.Diagnostics;
using DMS.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    public class HomeController : Controller
    {
        // display homeIndex depending on user role
        [HttpGet]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("AdminIndex"); //create AdminIndex view
            }

            return View("UserIndex"); //create UserIndex view
        }

        [HttpGet]
        public IActionResult About()
        {
            return View("About"); //create About View
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View("Contact"); //create Contact View
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
