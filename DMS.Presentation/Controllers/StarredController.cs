using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.StarredViews;
using DMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Presentation.Controllers
{
    [Authorize]
    public class StarredController : Controller
    {
        private readonly IStarredService starredService;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public StarredController(IStarredService starredService)
        {
            this.starredService = starredService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(StarredFilterViewModel fromRequest)
        {
            fromRequest.UserId = UserId;

            if (ModelState.IsValid)
            {
                var model = await starredService.GetStarOverviewAsync(fromRequest);
                return View("Index", model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadFolders(StarredFilterViewModel fromRequest)
        {
            fromRequest.UserId = UserId;
            if (ModelState.IsValid)
            {
                var model = await starredService.GetStarFoldersAsync(fromRequest);
                return PartialView("_StarredFoldersPartial", model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadDocuments(StarredFilterViewModel fromRequest)
        {
            fromRequest.UserId = UserId;
            if (ModelState.IsValid)
            {
                var model = await starredService.GetStarDocumentsAsync(fromRequest);
                return PartialView("_StarredDocumentsPartial", model);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> UnStarFolder(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { success = false, message = "Invalid folder ID." });

            Folder? folder = await starredService.GetFolderByIdAsync(id, UserId);
            if(folder == null)
                return NotFound(new { success = false, message = "Folder not found. or can not access" });

            try
            {
                starredService.UnStar(folder);
                return Json(new { success = true, message = "Folder UnStarred successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> UnStarDocument(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { success = false, message = "Invalid folder ID." });

            Document? document = await starredService.GetDocumentByIdAsync(id, UserId);
            if (document == null)
                return NotFound(new { success = false, message = "Document not found. or can not access" });

            try
            {
                starredService.UnStar(document);
                return Json(new { success = true, message = "Document UnStarred successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
