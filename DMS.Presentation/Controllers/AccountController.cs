using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.Account;
using DMS.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace DMS.Presentation.Controllers
{
    public class AccountController(IAccountService accountService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : Controller
    {

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login"); 
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel userFromRequest)
        {
            if (!ModelState.IsValid) return View(userFromRequest);

            var result = await accountService.LoginAsync(userFromRequest);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            if (result.IsLockedOut)
                return View("Locked");

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(userFromRequest);
        }
        [HttpPost]
        public async Task<IActionResult> GoogleLogin(string returnUrl = "/")
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { ReturnUrl = returnUrl });
            var props = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(props, "Google");
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction("Login");
            var linkedUser = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (linkedUser != null && linkedUser.IsLocked)
            {
                return View("Locked");
            }

            // --- Call the service to handle the complex logic ---
            var result = await accountService.ExternalLoginSignInAsync(info, returnUrl);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            // Final check for the custom lock on users identified by email (not key)
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
            var emailUser = await userManager.FindByEmailAsync(email);

            if (emailUser != null && emailUser.IsLocked)
            {
                return View("Locked");
            }

            // If all else failed (generic failure from service), redirect to login
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            accountService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel userFromRequest)
        {
            if (!ModelState.IsValid) return View(userFromRequest);

            //var baseUrl = "https://transthalamic-hans-unflauntingly.ngrok-free.dev";
            var baseUrl = "https://localhost:44389";
            var result = await accountService.RegisterAsync(userFromRequest, baseUrl);

            if (result.Succeeded)
                return RedirectToAction("ConfirmNotice");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(userFromRequest);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return View("ConfirmFail");

            var result = await accountService.ConfirmEmailAsync(userId, token);

            // This view just shows a success/fail message, no login required yet
            return View(result ? "ConfirmSuccess" : "ConfirmFail");
        }


        [HttpGet]
        public IActionResult ConfirmNotice()
        {
            return View();
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
