using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.TrashViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Presentation.Controllers
{
    [Authorize]
    public class TrashController : Controller
    {
        private readonly ITrashService trashService;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public TrashController(ITrashService trashService)
        {
            this.trashService = trashService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(TrashFilterViewModel fromRequest)
        {
            fromRequest.UserId = UserId;

            if(ModelState.IsValid)
            {
                var model = await trashService.GetTrashOverviewAsync(fromRequest);
                return View("Index", model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadFolders(TrashFilterViewModel fromRequest)
        {
            fromRequest.UserId = UserId;

            if(ModelState.IsValid)
            {
                var model = await trashService.GetTrashedFolderAsync(fromRequest);
                return PartialView("_TrashedFoldersPartial", model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadDocuments(TrashFilterViewModel fromRequest)
        {
            fromRequest.UserId = UserId;

            if(ModelState.IsValid)
            {
                var model = await trashService.GetTrashedDocumentAsync(fromRequest);
                return PartialView("_TrashedDocumentsPartial", model);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> RestoreFolder(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { success = false, message = "Invalid folder ID." });

            var folder = await trashService.GetFolderByIdAsync(id, UserId);
            if (folder == null)
                return NotFound(new { success = false, message = "Folder not found. or can not access" });

            try
            {
                await trashService.RestoreFolderAsync(folder);
                return Json(new { success = true, message = "Folder restored successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreDocument(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                return BadRequest(new {success = false, message = "Invalid Document Id."});

            Document? document = await trashService.GetDocumentByIdAsync(id, UserId);
            if(document == null)
                return NotFound(new { success = false, message = "Document Not Found. Or can not access" });
            try
            {
                await trashService.RestoreDocumentAsync(document);
                return Json(new { success = true, message = "Folder restored successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFolder(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { success = false, message = "Invalid folder ID." });

            var folder = await trashService.GetFolderByIdAsync(id, UserId);
            if (folder == null)
                return NotFound(new { success = false, message = "Folder not found. Or can not access" });

            try
            {
                await trashService.DeleteFolderAsync(id, UserId);
                return Json(new { success = true, message = "Folder deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                return BadRequest(new {success = false, message = "Invalid Document Id."});

            Document? document = await trashService.GetDocumentByIdAsync(id, UserId);
            if(document == null)
                return NotFound(new { success = false, message = "Document Not Found. Or can not access" });
            try
            {
                string wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                await trashService.DeleteDocumentAsync(id, UserId, wwwroot);
                return Json(new { success = true, message = "Folder deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
