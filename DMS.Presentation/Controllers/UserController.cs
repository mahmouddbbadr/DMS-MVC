using DMS.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }
        // Get all users
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        // Details of a user
        [HttpGet]
        public IActionResult Details(string id)
        {
            return View();
        }
        //open form to create user
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // save data and create user
        // instead of (object model) you will create UserViewModel and pass it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(object model)//(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Unlock user logic

            return RedirectToAction(nameof(Index));
        }
        // open form to edit user
        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View();
        }
        // save editing
        // instead of (object model) you will use UserViewModel that you created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(object model)//(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Unlock user logic

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            // Unlock user logic

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LockUser(string id)
        {
            // Unlock user logic

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnlockUser(string id)
        {
            // Unlock user logic

            return RedirectToAction(nameof(Index));
        }

    }
}
