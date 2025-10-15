using Microsoft.AspNetCore.Mvc;

namespace DMS.Presentation.Controllers
{
    public class FolderController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
