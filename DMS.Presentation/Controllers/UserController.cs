using DMS.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController(IUserService userService) : Controller
    {
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> Unblocked(string? searchEmail, int page = 1, int pageSize = 8)
        {

            var result = string.IsNullOrEmpty(searchEmail)
                ? await userService.GetAllUnBlockedAsnyc(UserId, page, pageSize)
                : await userService.SearchUnBlockedUsersAsnyc(UserId, searchEmail, page, pageSize);

            ViewBag.TotalCount = result.totalCount;
            ViewBag.TotalPages = result.totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.SearchEmail = searchEmail;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserListPartial", result.users);
            }
            return View("Unblocked", result.users);
        }



        [HttpGet]
        public async Task<IActionResult> Blocked(string? searchEmail, int page = 1, int pageSize = 8)
        {
            var result = string.IsNullOrEmpty(searchEmail)
                ? await userService.GetAllBlockedAsnyc(page, pageSize)
                : await userService.SearchBlockedUsersAsnyc(searchEmail, page, pageSize);

            ViewBag.TotalCount = result.totalCount;
            ViewBag.TotalPages = result.totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.SearchEmail = searchEmail;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserListPartial", result.users);
            }

            return View("Blocked", result.users);
        }


        [HttpPost]
        public async Task<IActionResult> Block(string email)
        {
            if (await userService.BlockUserAsnyc(email))
                return Ok();

            return BadRequest("Failed to block user.");
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string email)
        {
            if (await userService.UnBlockUserAsnyc(email))
                return Ok();

            return BadRequest("Failed to unblock user.");
        }
    }
}