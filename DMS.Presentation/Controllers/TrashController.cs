using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.TrashViews;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Presentation.Controllers
{
    public class TrashController : Controller
    {
        private readonly ITrashService trashService;

        public TrashController(ITrashService trashService)
        {
            this.trashService = trashService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(TrashFilterViewModel fromRequest)
        {
            if(ModelState.IsValid)
            {
                var model = await trashService.GetTrashOverviewAsync
                    (
                        fromRequest.SearchTerm,
                        fromRequest.PageNum,
                        fromRequest.PageSize,
                        fromRequest.SortField!,
                        fromRequest.SortOrder!
                    );
                return View("Index", model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadFolders(TrashFilterViewModel fromRequest)
        {
            if(ModelState.IsValid)
            {
                var model = await trashService.GetTrashedFolderAsync
                    (
                        fromRequest.SearchTerm,
                        fromRequest.PageNum,
                        fromRequest.PageSize,
                        fromRequest.SortField!,
                        fromRequest.SortOrder!
                    );
                return PartialView("_TrashedFoldersPartial", model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadDocuments(TrashFilterViewModel fromRequest)
        {
            if(ModelState.IsValid)
            {
                var model = await trashService.GetTrashedDocumentAsync
                    (
                        fromRequest.SearchTerm,
                        fromRequest.PageNum,
                        fromRequest.PageSize,
                        fromRequest.SortField!,
                        fromRequest.SortOrder!
                    );
                return PartialView("_TrashedDocumentsPartial", model);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> RestoreFolder(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { success = false, message = "Invalid folder ID." });

            var folder = await trashService.GetFolderByIdAsync(id);
            if (folder == null)
                return NotFound(new { success = false, message = "Folder not found." });

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

            Document? document = await trashService.GetDocumentByIdAsync(id);
            if(document == null)
                return NotFound(new { success = false, message = "Document Not Found." });
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

            var folder = await trashService.GetFolderByIdAsync(id);
            if (folder == null)
                return NotFound(new { success = false, message = "Folder not found." });

            try
            {
                await trashService.DeleteFolderAsync(id);
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

            Document? document = await trashService.GetDocumentByIdAsync(id);
            if(document == null)
                return NotFound(new { success = false, message = "Document Not Found." });
            try
            {
                await trashService.DeleteDocumentAsync(id);
                return Json(new { success = true, message = "Folder deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
