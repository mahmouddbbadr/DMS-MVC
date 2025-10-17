using DMS.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentService _documentService)
        {
            documentService = _documentService;
        }

        // Get all documents based on their folderId
        [HttpGet]
        public IActionResult Index(string folderId)
        {        
            return View();
        }

        // Details of a Document
        [HttpGet]
        public IActionResult Details(string id)
        {
            return View();
        }

        // open form to Upload new document
        [HttpGet]
        public IActionResult Upload(string folderId)
        {
            return View();
        }

        // save data and upload a document
        // instead of (object obj) you will create DocumentUploadViewModel and pass it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(object obj)
        {
            return View();
        }

        // download the document in your pc
        [HttpGet]
        public IActionResult Download(string id)
        {
            return View();
        }

        // open form to edit a document
        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View();
        }

        // save editing
        // instead of (object obj) you will use DocumentViewModel that you created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(object obj)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Star(string id)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnStar(string id)
        {
            return RedirectToAction("Index");
        }
    }
}

