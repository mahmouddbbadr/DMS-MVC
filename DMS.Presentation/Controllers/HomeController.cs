using System.Diagnostics;
using System.Threading.Tasks;
using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Presentation.Models;
using DMS.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashBoardService dashboardService;

        public HomeController(DashBoardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }



        // display homeIndex depending on user role
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("Admin"))
            {
                var model = await dashboardService.GetAdminStats(userId);

                return View("AdminIndex", model); //create AdminIndex view
            }
            else
            {
                var model = await dashboardService.GetUserStats("user-2");

                return View("UserIndex", model); //create UserIndex view
                //return View("AdminIndex", model); //create AdminIndex view
            }

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

        [HttpPost]
        public IActionResult Contact(string name, string email, string subject, string message)
        {
            
            TempData["SuccessMessage"] = "Thank you for contacting us! We'll get back to you soon.";

            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


     
    }
}
