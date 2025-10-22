using DMS.Service.IService;
using DMS.Service.ModelViews.SharedViewModel;
using DMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMS.Presentation.Controllers
{
    //[Authorize]
    public class SharingController : Controller
    {
        private readonly ISharingService sharingService;

        public SharingController(ISharingService _sharingService)
        {
            sharingService = _sharingService;
        }

        // display all folders/documents that shared with/by me
        [AllowAnonymous]

        [HttpGet]
        public async Task <IActionResult> Index(string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 5)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (sharedWithMeItems, totalWithMe) = await sharingService.GetSharedWithMeAsync(userId, search, sortOrder, page, pageSize);
            var (sharedByMeItems, totalByMe) = await sharingService.GetSharedByMeAsync(userId, search, sortOrder, page, pageSize);

            var viewModel = new SharedIndexViewModel
            {
                SharedWithMe = sharedWithMeItems,
                SharedByMe = sharedByMeItems,
                SearchTerm = search,
                SortOrder = sortOrder,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPagesWithMe = (int)Math.Ceiling(totalWithMe / (double)pageSize),
                TotalPagesByMe = (int)Math.Ceiling(totalByMe / (double)pageSize)
            };

            return View(viewModel);

        }

        // open form to share folder with someone
        [HttpGet]
        public IActionResult ShareFolder(string folderId)
        {

            var model = new ShareViewModel { FolderId = folderId };

            return View(model);
        }

        // open form to share document with someone
        [HttpGet]
        public IActionResult ShareDocument(string documentId)
        {
            var model = new ShareViewModel { DocumentId = documentId };
            return View(model);
        }

        // save sharing
        // instead of (object model) you will create ShareViewModel and pass it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Share(ShareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model.FolderId != null ? "ShareFolder" : "ShareDocument", model);
            }

            model.SharedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.FolderId != null)
            {
                await sharingService.ShareFolderAsync(model);
            }
            else if (model.DocumentId != null)
            {
                await sharingService.ShareDocumentAsync(model);
            }

            return RedirectToAction(nameof(Index));


        }

        // unshare folder/document
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unshare(string shareItemId)
        {
            await sharingService.UnshareAsync(shareItemId);
            return RedirectToAction(nameof(Index));
        }
    }
}
