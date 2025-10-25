using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService documentService;
        private readonly string directory;
        public DocumentController(IDocumentService _documentService)
        {
            documentService = _documentService;
            directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        // Get all documents based on their folderId
        //[HttpGet]
        //public async Task<IActionResult> Index(string folderId)
        //{ 
        //    if(!string.IsNullOrEmpty(folderId))
        //    {
        //        DocumentIndexViewModel model = await documentService
        //            .GetDocumentsByFolderIdAsync(folderId);

        //        return View("Index", model);
        //    }
        //    return NotFound();
        //}

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DocumentQueryViewModel query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = await documentService.GetDocumentsByFolderIdWithPaginationAsync(
                query.FolderId,
                query.SearchName,
                query.PageNum,
                query.PageSize,
                query.SortField,
                query.SortOrder
            );

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_IndexPartialView", model);
            }

            return View("Index", model);
        }

        // Details of a Document
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest();

            DocumentDetailsViewModel? model = await documentService
                .GetDocumentDetailsAsync(id);
           
            if(model == null)
                return NotFound();
        
            return View("Details", model);
        }

        // open form to Upload new document
        [HttpGet]
        public IActionResult Upload(string folderId)
        {
            DocumentUploadViewModel model = new() 
            {       
                FolderId = folderId
            };
            return View("Upload", model);
        }

        // save data and upload a document
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(DocumentUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(await documentService.UploadDocumentAsync(model, directory))
                    return RedirectToAction("Index", new { folderId = model.FolderId});

                ModelState.AddModelError("", "Something went wrong while uploading.");
            }
            return View("Upload", model);
        }

        // download the document in your pc
        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await documentService.GetFileToDownloadAsync(id, directory);

            if (result == null)
                return NotFound();

            return File(result.FileBytes, result.ContentType, result.FileName);
        }

        // open form to edit a document
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
                return BadRequest();

            Document? doc = await documentService.GetDocumentByIdAsync(id);
            if (doc == null)
                return NotFound();

            DocumentUploadViewModel model = new()
            {
                Id = id,
                Name = doc.Name,
                FolderId = doc.FolderId,
                ExistingFilePath = doc.FilePath
            };

            return View("Edit", model);
        }

        // save editing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DocumentUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(await documentService.EditDocumentAsync(model, directory))
                    return RedirectToAction("Index", new { folderId = model.FolderId});

                ModelState.AddModelError("", "Something went wrong while Updating.");
            }
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            Document? document = await documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound();

            try
            {
                await documentService.DeleteDocumentAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Trash(string id)
        {
            bool trashed = false;

            if (!string.IsNullOrEmpty(id))
            {
                trashed = await documentService.TrashDocumentAsync(id);
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
                await documentService.StarDocumentAsync(id, true);
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
                await documentService.StarDocumentAsync(id, false);
                return Ok();
            }
            return BadRequest();
        }
    }
}

