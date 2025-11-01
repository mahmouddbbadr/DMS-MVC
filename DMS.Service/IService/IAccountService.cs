using DMS.Service.ModelViews.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.IService
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterUserViewModel model, string confirmLinkBase);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<SignInResult> LoginAsync(LoginUserViewModel model);
        Task LogoutAsync();
        Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info, string returnUrl);
    }
}
