using System.Diagnostics;
using DMS.Service.IService;
using DMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashBoardService dashboardService;

        public HomeController(IDashBoardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }
            if (User.IsInRole("Admin"))
            {
                var model = await dashboardService.GetAdminStats(userId);

                return View("AdminIndex", model);
            }
            else
            {
                var model = await dashboardService.GetUserStats(userId);

                return View("UserIndex", model);
            }
        }

        [HttpGet]
        public IActionResult About()
        {
            return View("About");
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View("Contact");
        }

        [HttpPost]
        public IActionResult Contact(string name, string email, string subject, string message)
        {
            
            TempData["SuccessMessage"] = "Thank you for contacting us! We'll get back to you soon.";

            return RedirectToAction("Contact");
        }

    }
}
