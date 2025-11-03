using AutoMapper;
using DMS.Domain.ENums;
using DMS.Domain.Models;
using DMS.Service.IService;
using DMS.Service.ModelViews.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace DMS.Service.Service
{
    public class AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IMapper mapper ) : IAccountService
    {
        public async Task<IdentityResult> RegisterAsync(RegisterUserViewModel model, string confirmLinkBase)
        {
            var existingUser = await userManager.FindByEmailAsync(model.EmailAddress);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "This email is already registered."
                });
            }

            var user = mapper.Map<AppUser>(model);
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return result;

            await userManager.AddToRoleAsync(user, "User");
            await signInManager.SignInAsync(user, isPersistent: false);


            // 5️⃣ Generate email confirmation token and send email
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmLink = $"{confirmLinkBase}/Account/ConfirmEmail?userId={user.Id}&token={encodedToken}";
            await emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{confirmLink}'>Confirm Email</a>");

            return result;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }

        public async Task<SignInResult> LoginAsync(LoginUserViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return SignInResult.Failed;

            if (user.IsLocked)
                return SignInResult.LockedOut;

            // Use PasswordSignInAsync to automatically handle lockouts and failures
            var result = await signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            await userManager.AddClaimAsync(user, new Claim("FName", user.FName ?? ""));
            await userManager.AddClaimAsync(user, new Claim("LName", user.LName ?? ""));

            return result;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info, string returnUrl)
        {
            // The full logic from your controller goes here.

            // Attempt to sign in the existing user linked to this Google account.
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            // 1. SUCCESSFUL SIGN-IN
            if (result.Succeeded)
            {
                var signedInUser = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                // CUSTOM CHECK 1: User is linked and signed in, but check custom lock.
                if (signedInUser != null && signedInUser.IsLocked)
                {
                    // Cannot use RedirectToAction here, return a failure status or specialized result.
                    // For simplicity, we'll return a dedicated SignInResult (a custom one or just Failed).
                    await signInManager.SignOutAsync();

                    // Since SignInResult doesn't have a built-in "CustomLocked" status, 
                    // we'll return Failed and handle the lock check in the controller.
                    return SignInResult.Failed;
                }
                return result; // Successful sign-in result
            }

            // Extract basic information from claims
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);

            // 2. ACCOUNT LINKING: Check if a local account exists with the same email.
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // Custom Lock Check for linking user
                if (user.IsLocked)
                {
                    return SignInResult.Failed; // Return failed, controller handles the redirect to LockedCustom
                }

                // Link the account
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                }
                await userManager.AddLoginAsync(user, info);
                await signInManager.SignInAsync(user, isPersistent: false);
                return SignInResult.Success; // Successfully linked and signed in
            }

            // 3. NEW ACCOUNT CREATION
            if (string.IsNullOrEmpty(firstName))
            {
                var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);
                firstName = fullName?.Split(' ')[0] ?? "User";
            }

            var newUser = new AppUser
            {
                UserName = email,
                Email = email,
                FName = firstName,
                LName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "Last",
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(newUser);

            if (createResult.Succeeded)
            {
                await userManager.AddLoginAsync(newUser, info);
                await userManager.AddToRoleAsync(newUser, "User");
                await signInManager.SignInAsync(newUser, false);
                return SignInResult.Success; // Successfully created and signed in
            }

            // Return Failed, the controller can use the info.Principal to display error messages if needed.
            return SignInResult.Failed;
        }
    }
}
