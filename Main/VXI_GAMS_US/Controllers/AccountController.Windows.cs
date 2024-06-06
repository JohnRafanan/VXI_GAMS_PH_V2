using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using VXI_GAMS_US.Models;

namespace VXI_GAMS_US.Controllers
{
    [Authorize]
    public partial class AccountController
    {
        //
        // POST: /Account/WindowsLogin
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> WindowsLogin(string userName, string attUid, string returnUrl)
        {
            if (!Request.LogonUserIdentity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return await Login(new LoginViewModel
            {
                NTAccount = Request.LogonUserIdentity.Name?.ToLower(),
                AttUID = attUid
            }, returnUrl);
        }
        
        //
        // POST: /Account/WindowsLogOff
        [HttpPost]
        public void WindowsLogOff()
        {
            AuthenticationManager.SignOut();
        }

        //
        // POST: /Account/LinkWindowsLogin
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> LinkWindowsLogin()
        {
            string userId = HttpContext.ReadUserId();

            //didn't get here through handler
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            HttpContext.Items.Remove("windows.userId");

            //not authenticated.
            var loginInfo = GetWindowsLoginInfo();
            if (loginInfo == null)
                return RedirectToAction("Manage");

            //add linked login
            var result = await UserManager.AddLoginAsync(userId, loginInfo);

            //sign the user back in.
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
                await SignInAsync(user, false);

            if (result.Succeeded)
                return RedirectToAction("Manage");

            return RedirectToAction("Manage", new { Message = ManageController.ManageMessageId.Error });
        }

        #region helpers
        private UserLoginInfo GetWindowsLoginInfo()
        {
            if (!Request.LogonUserIdentity.IsAuthenticated)
                return null;

            return new UserLoginInfo("Windows", Request.LogonUserIdentity.User.ToString());
        }
        #endregion
    }
}