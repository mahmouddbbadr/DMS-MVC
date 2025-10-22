//using DMS.Service.IService;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

//namespace DMS.Presentation.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class RoleController : Controller
//    {
//        private readonly IRoleService roleService;

//        public RoleController(IRoleService _roleService)
//        {
//            roleService = _roleService;
//        }

//        // get all roles
//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        // Details of a role
//        [HttpGet]
//        public IActionResult Details(string id)
//        {
//            return View();
//        }

//        // open form to create new role
//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // save data and create a new role
//        // instead of (object model) you will create RoleViewModel and pass it
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(object model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            // Add role logic here...

//            return RedirectToAction("Index");
//        }

//        // open form to edit role
//        [HttpGet]
//        public IActionResult Edit(string id)
//        {
//            return View();
//        }

//        // save editing
//        // instead of (object model) you will use RoleViewModel that you created
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(object model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            // Add logic here...

//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Delete(string id)
//        {
//            // Add logic here...

//            return RedirectToAction("Index");
//        }
//    }
//}
