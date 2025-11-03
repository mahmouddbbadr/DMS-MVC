using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using DMS.Service.ModelViews.Shared;
using DMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMS.Presentation.Controllers
{
    [Authorize]
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
        public async Task <IActionResult> SharedWithMe(string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Console.WriteLine("Current user: " + userId);


            var sharedWithMeItems = await sharingService.GetSharedWithMeAsync(userId, search, sortOrder, page, pageSize);

            return View("SharedWithMe", sharedWithMeItems);

        }

        [HttpGet]
        public async Task<IActionResult> SharedByMe(string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sharedByMeItems = await sharingService.GetSharedByMeAsync(userId, search, sortOrder, page, pageSize);

            return View("SharedByMe", sharedByMeItems);

        }

        // open form to share folder with someone
        [HttpGet]
        public IActionResult ShareFolder(string folderId)
        {
            if (string.IsNullOrWhiteSpace(folderId))
            {
                return BadRequest("Folder ID is required.");
            }

            var model = new ShareViewModel { FolderId = folderId };
            return View(model);
        }


        // open form to share document with someone
        [HttpGet]
        public IActionResult ShareDocument(string documentId)
        {
            if (documentId != null)
            {

                var model = new ShareViewModel { DocumentId = documentId };
                return View(model);
            }
            else
            {
                return BadRequest();
            }
        }

        // save sharing
        // instead of (object model) you will create ShareViewModel and pass it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Share(ShareViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                string invalidView = model.FolderId != null ? "ShareFolder" :
                                     model.DocumentId != null ? "ShareDocument" : "ShareError";
                return View(invalidView, model);
            }
            model.SharedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine("Current User Id: " + currentUserId);
            try
            {
                if (model.FolderId != null)
                {
                    await sharingService.ShareFolderAsync(model);
                }
                else if (model.DocumentId != null)
                {
                    await sharingService.ShareDocumentAsync(model);
                }

                return RedirectToAction(nameof(SharedByMe));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model.FolderId != null ? "ShareFolder" : "ShareDocument", model);
            }

        }

        // unshare folder/document
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unshare(string shareItemId)
        {
            if (string.IsNullOrEmpty(shareItemId))
                return BadRequest("Share item ID is required.");

            try
            {
                await sharingService.UnshareAsync(shareItemId);
                TempData["SuccessMessage"] = "Item unshared successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(SharedByMe));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnshareAll(string id, string type)
        {

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(type))
            {
                TempData["Error"] = "Invalid request.";
                return RedirectToAction("SharedByMe");
            }

            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var count = await sharingService.UnshareAllAsync(id, type, currentUserId);

                TempData["Success"] = $"Successfully unshared with {count} user(s).";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("SharedByMe");
        }
        [HttpGet]
        public async Task<IActionResult> UnshareSelection(string id, string type)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(type))
                {
                    TempData["Error"] = "Item ID and Type are required.";
                    return RedirectToAction("SharedByMe");
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var model = await sharingService.GetSharedUsersAsync(id, type, currentUserId);

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading shared users: {ex.Message}";
                return RedirectToAction("SharedByMe");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnshareSelection(UnshareSelectionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.SelectedUserIds == null || !model.SelectedUserIds.Any())
                {
                    TempData["Error"] = "Please select at least one user to unshare.";
                    return View(model);
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ;
                var unsharedCount = await sharingService.UnshareWithUsersAsync(
                    model.ItemId, model.Type, model.SelectedUserIds, currentUserId);

                if (unsharedCount > 0)
                {
                    TempData["Success"] = $"Successfully unshared {model.ItemName} with {unsharedCount} user(s).";
                }
                else
                {
                    TempData["Error"] = "No users were unshared. Please check your selection.";
                }

                return RedirectToAction("SharedByMe");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error unsharing: {ex.Message}";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnshareSingle(string itemId, string type, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "Invalid request parameters.";
                    return RedirectToAction("SharedByMe", "Share");
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var success = await sharingService.UnshareWithUserAsync(itemId, type, userId, currentUserId);

                if (success)
                {
                    var userEmail = await sharingService.GetUserEmailAsync(userId);
                    TempData["Success"] = $"Successfully unshared with {userEmail}.";
                }
                else
                {
                    TempData["Error"] = "Failed to unshare with the selected user.";
                }

                return RedirectToAction("UnshareSelection", new { id = itemId, type = type });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error unsharing: {ex.Message}";
                return RedirectToAction("UnshareSelection", new { id = itemId, type = type });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Folder([FromQuery] FolderSharedViewModel fromReq)
        {
            fromReq.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool authorized = await sharingService
                .IsAuthorizedSharedFolderAsync(fromReq.FolderId, fromReq.OwnerId);
            if (!authorized)
                return NotFound();

            var model = await sharingService
                .GetDocumentsBySharedFolderIdWithPaginationAsync(fromReq);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_FolderPartialView", model);
            }

            return View("Folder", model);
        }
    }
}
