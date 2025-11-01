using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using DMS.Service.ModelViews.Folder;
using DMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DMS.Presentation.Controllers
{
    [Authorize]
    public class FolderController : Controller
    {
        private readonly IFolderService folderService;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public FolderController(IFolderService _folderService)
        {
            folderService = _folderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] FolderQueryViewModel query)
        {
            query.OwnerId = UserId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = await folderService
                .GetFoldersWithPaginationAsync(query);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_IndexPartialView", model);
            }

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FolderCreateViewModel model)
        {
            model.OwnerId = UserId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await folderService.CreateAsync(model);
            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var folder = await folderService.GetFolderAsync(id, UserId);
            if (folder == null) return NotFound();
            return Json(new { folder.Id, folder.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FolderEditViewModel fromRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await folderService.RenameFolderAsync(fromRequest, UserId);

            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Trash(string id)
        {
            bool trashed = false;

            if (!string.IsNullOrEmpty(id))
            {
                trashed = await folderService.TrashFolderAsync(id, UserId);
            }
            if (trashed)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Star(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await folderService.StarFolderAsync(id, UserId, true);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unstar(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await folderService.StarFolderAsync(id, UserId, false);
                return Ok();
            }
            return BadRequest();
        }

    }
}
