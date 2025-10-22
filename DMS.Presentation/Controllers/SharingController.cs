//using DMS.Service.IService;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace DMS.Presentation.Controllers
//{
//    [Authorize]
//    public class SharingController : Controller
//    {
//        private readonly ISharingService sharingService;

//        public SharingController(ISharingService _sharingService)
//        {
//            sharingService = _sharingService;
//        }

//        // display all folders/documents that shared with/by me
//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        // open form to share folder with someone
//        [HttpGet]
//        public IActionResult ShareFolder(string folderId)
//        {
//            return View();
//        }

//        // open form to share document with someone
//        [HttpGet]
//        public IActionResult ShareDocument(string documentId)
//        {
//            return View();
//        }

//        // save sharing
//        // instead of (object model) you will create ShareViewModel and pass it
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Share(object model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View("Share", model);
//            }
//            return View();
//        }

//        // unshare folder/document
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Unshare(string shareItemId)
//        {
//            return View();
//        }
//    }
//}
