using DMS.Service.IService;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using System;

namespace DMS.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        ///<summary>
        /// open form to login
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login"); //create Login View
        }

        ///<summary>
        /// check Authentecation
        ///</summary>
        /// instead of (object model) you will create LoginViewModel and pass it
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(object model)
        {
            if (!ModelState.IsValid)
                return View("Login",model);

            // TODO: Handle login logic

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            // TODO: Sign out user
            return RedirectToAction("Login");
        }

        // open form to register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register"); // create Register view
        }

        // save registeration and create new user
        // instead of (object model) you will create RegisterViewModel and pass it
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(object model)
        {
            if (!ModelState.IsValid)
                return View("Register", model); 

            // TODO: Register user

            return RedirectToAction("Login");
        }

        //open password Reset Request Page
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword"); //create ForgotPassword view
        }

        // Handle Request
        // instead of (object model) you will create ForgotPasswordViewModel and pass it
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(object model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: Send password reset email

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            //Find the user by email
            //Check if user exists and is confirmed
            //Generating the password reset token for the user
            //Creating the reset password URL containing the token
            //Sending that URL to the user’s email
            //Redirect to confirmation page
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        // open form to reset password
        // instead of object you will create ResetPasswordViewModel{token,email}
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            //var model = new ResetPasswordViewModel { Token = token };
            return View();//(model);
        }

        // save editing in database
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(object model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: Reset user password using token

            return RedirectToAction("Login");
        }
    }
}


/// DMS.Presentation
//│
//├── Controllers
//│   └── AccountController.cs
//│
//├── ViewModels
//│   └── Account
//│       ├── LoginViewModel.cs
//│       ├── RegisterViewModel.cs
//│       ├── ForgotPasswordViewModel.cs
//│       └── ResetPasswordViewModel.cs
//│
//├── Views
//│   └── Account
//│       ├── Login.cshtml
//│       ├── Register.cshtml
//│       ├── ForgotPassword.cshtml
//│       ├── ForgotPasswordConfirmation.cshtml
//│       ├── ResetPassword.cshtml
//│       └── Logout.cshtml (optional)
