using DMS.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    [Authorize]
    public class FolderController : Controller
    {
        private readonly IFolderService folderService;

        public FolderController(IFolderService _folderService)
        {
            folderService = _folderService;
        }

        // if you do not pass parentFolderId it will display all base folders
        // if you pass parentFolderId it will display subfolders of this parentFolderId
        [HttpGet]
        public IActionResult Index(string? parentFolderId)
        {
            return View();
        }

        // Details of a Folder
        [HttpGet]
        public IActionResult Details(string id)
        {
            return View();
        }

        //open form to create a Folder
        [HttpGet]
        public IActionResult Create(string? parentFolderId)
        {
            return View();
        }

        // save data and create a new folder
        // instead of (object obj) you will create FolderViewModel and pass it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(object obj)//(FolderViewModel model)
        {
            return View();
        }

        // open form to edit folder
        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View();
        }

        // save editing
        // instead of (object obj) you will use FolderViewModel that you created
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(object obj)//(FolderViewModel model)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            // Delete logic
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Star(string id)
        {
            // Star logic
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Unstar(string id)
        {
            // Unstar logic
            return RedirectToAction("Index");
        }

    }
}
