using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace DMS.Presentation.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentService documentService;
        private readonly string directory;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        public DocumentController(IDocumentService _documentService)
        {
            documentService = _documentService;
            directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DocumentQueryViewModel query)
        {
            query.OwnerId = UserId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool authorized = await documentService
                .IsAuthorizedFolderAsync(query.FolderId, UserId);
            if (!authorized)
                return NotFound();

            var model = await documentService
                .GetDocumentsByFolderIdWithPaginationAsync(query);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_IndexPartialView", model);
            }

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            DocumentDetailsViewModel? model = await documentService
                .GetDocumentDetailsAsync(id, UserId);
           
            if(model == null)
                return NotFound();
        
            return View("Details", model);
        }

        [HttpGet]
        public async Task<IActionResult> Upload(string folderId)
        {
            if(string.IsNullOrEmpty(folderId))
                return BadRequest();

            bool authorized = await documentService
                .IsAuthorizedFolderAsync(folderId, UserId);
            if (!authorized)
                return NotFound();

            DocumentUploadViewModel model = new() 
            {       
                FolderId = folderId,
                OwnerId = UserId
            };
            return View("Upload", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(DocumentUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await documentService.UploadDocumentAsync(model, directory);

                if (result.Success)
                    return RedirectToAction("Index", new { folderId = model.FolderId });

                if (result.NameExists)
                {
                    ModelState.AddModelError("Name", "A document with this name already exists in this folder.");
                    return View("Upload", model);
                }

                ModelState.AddModelError("", "Something went wrong while uploading.");
            }

            return View("Upload", model);
        }


        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await documentService.GetFileToDownloadAsync(id, UserId, directory);

            if (result == null)
                return NotFound();

            return File(result.FileBytes, result.ContentType, result.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(DocumentEditViewModel fromRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            DocumentUploadViewModel? model = await documentService.
                SetEditDocumentAsync(fromRequest, UserId);

            if (model == null)
                return NotFound();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DocumentUploadViewModel model)
        {
            
            if (!ModelState.IsValid)
                return View("Edit", model);

            bool updated = await documentService.EditDocumentAsync(model, directory);

            if (!updated)
            {
                ModelState.AddModelError("", "Something went wrong while updating.");
                return View("Edit", model);
            }

            if (!string.IsNullOrWhiteSpace(model.ReturnURL))
                return Redirect(model.ReturnURL);

            return RedirectToAction("Index", new { folderId = model.FolderId });
        }

        [HttpPost]
        public async Task<IActionResult> EditModal(DocumentEditModelViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid Document"});

            model.OwnerId = UserId;

            try
            {
                bool updated = await documentService.EditDocumentModelAsync(model, directory);

                if (updated)
                    return Json(new { success = true, message = "Document updated successfully." });
                return Json(new { success = false, message = "Document Not Found." });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Invalid document id.");

            var document = await documentService.GetDocumentByIdAsync(id, UserId);

            if (document == null)
                return NotFound("Document not found or you don't have access.");

            try
            {
                await documentService.DeleteDocumentAsync(id, UserId); 
                return Ok(new { success = true, message = "Document deleted successfully." });
            }
            catch
            {
                
                return StatusCode(500, "An error occurred while deleting the document.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Trash(string id)
        {
            bool trashed = false;

            if (!string.IsNullOrEmpty(id))
            {
                trashed = await documentService.TrashDocumentAsync(id, UserId);
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
                await documentService.StarDocumentAsync(id, UserId, true);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnStar(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await documentService.StarDocumentAsync(id, UserId, false);
                return Ok();
            }
            return BadRequest();
        }

    }
}

