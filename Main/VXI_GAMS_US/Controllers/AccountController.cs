using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.Models;
using VXI_GAMS_US.VIEWS.Entities;
using VXI_GAMS_US.VIEWS.View;
// ReSharper disable BadControlBracesIndent

namespace VXI_GAMS_US.Controllers
{
    [Authorize]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> AddLogin(string region, string role, string accounts)
        {
            var errors = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(region))
                    throw new Exception("Region is required");
                region = Regex.Replace(region, "[^a-zA-Z]", "");
                region = region.ToUpper();
                accounts = Regex.Replace(accounts, "[^a-zA-Z0-9,]", "");
                if (string.IsNullOrEmpty(accounts) || accounts.Length < 1)
                    throw new Exception("Atleast one account must be entered");
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                    {
                        AllowOnlyAlphanumericUserNames = false,
                        RequireUniqueEmail = true
                    };
                    userManager.PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 1,
                        RequireNonLetterOrDigit = false,
                        RequireDigit = false,
                        RequireLowercase = false,
                        RequireUppercase = false,
                    };
                    foreach (var hrid in accounts.Split(','))
                    {
                        try
                        {
                            var model = new LoginViewModel
                            {
                                Username = $"{region}-{hrid}".Trim().ToUpper()
                            };
                            var user = await userManager.FindByNameAsync(model.Username);
                            if (user != null)
                                throw new Exception($"User: {model.Username} is already added");
                            var userResult = await userManager.CreateAsync(new ApplicationUser { UserName = model.Username, Email = model.Email }, model.Password);
                            if (!userResult.Succeeded)
                                throw new ApplicationException($"Creating User failed with error(s): {userResult.Errors}");
                            user = await userManager.FindByNameAsync(model.Username);
                            if (user == null)
                                throw new Exception($"Cannot add user: {model.Username}");
                            user.EmailConfirmed = true;
                            await userManager.UpdateAsync(user);
                            await userManager.AddToRoleAsync(user.Id, role);
                        }
                        catch (Exception e)
                        {
                            errors.Add(ExceptionHandler.GetMessages(e));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errors.Add(ExceptionHandler.GetMessages(e));
            }
            ViewBag.Message = errors.Aggregate(string.Empty, (current, error) => current + error + "\r\n");
            return View("Error");
        }

        /// <summary>
        /// Using https://api.vxiusa.com/api/GlobalHR/Employees/FindEEByWinIDDomain/ model
        /// </summary>
        /// <param name="hrid">The HRID of the Employee/User</param>
        /// <param name="enclose"></param>
        /// <returns></returns>
        public static async Task<ApiViewModel<Api>> GetEmployee(string hrid, bool enclose = false)
        {
            try
            {
                return await HttpHelper<ApiViewModel<Api>>.GetApiViewModelAccount(ConfigurationManager.AppSettings["IT_API"], hrid, enclose);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }

        /// <summary>
        /// Using https://api.vxiusa.com/api/GlobalHR/Employees/FindEEByWinIDDomain/ model
        /// </summary>
        /// <param name="hrid">The HRID of the Employee/User</param>
        /// <param name="enclose"></param>
        /// <returns></returns>
        public static async Task<ApiViewModel<Api>> GetEmployee2(string hrid, bool enclose = false)
        {
            try
            {
                return await HttpHelper<ApiViewModel<Api>>.GetApiViewModelAccount(ConfigurationManager.AppSettings["IT_API2"], hrid, enclose);
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                Console.WriteLine(error);
            }
            return null;
        }

        public async Task<IdentityResult> CreateAsync(RegisterViewModel model)
        {
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var result = await UserManager.CreateAsync(new ApplicationUser { UserName = model.Username, Email = model.Email }, model.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            var user = UserManager.FindByName(model.Username);
            user.Email = $"{user.UserName}@dont.use";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            UserManager.Update(user);
            UserManager.AddToRole(user.Id, model.Role);
            return result;
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var windowsId = (model.NTAccount ?? string.Empty).ToLower();
            model.Username = windowsId.Replace("\\", "-");
            model.AttUID = model.NTAccount = string.Empty;
            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("Username"))
                {
                    ModelState["Username"].Errors.Clear();
                    ModelState["Username"].Errors.Add(model.IsAdminForm ? "NT ACCOUNT is required" : "ATTUID is required");
                }
                return View("Login", model);
            }
            try
            {
                string ntDomain, tmpAccount;
                //windowsId = "vxiphp\\jtayag";
                try
                {
                    ntDomain = windowsId.Split(new[] { @"\" }, StringSplitOptions.None)[0];
                }
                catch
                {
                    ntDomain = "ZZZ";
                }
                try
                {
                    tmpAccount = windowsId.Split(new[] { @"\" }, StringSplitOptions.None)[1];
                }
                catch
                {
                    tmpAccount = "ZZZ";
                }
                var api = await GetEmployee2($"{tmpAccount}/{ntDomain}");
                if (string.IsNullOrEmpty(api?.Table?.FirstOrDefault()?.ID))
                {
                    api = await GetEmployee($"{tmpAccount}/{ntDomain}");
                    if (string.IsNullOrEmpty(api?.Table?.FirstOrDefault()?.ID))
                        throw new Exception("Data is not in Global Api");
                }
                var account = api.Table.First();
                model.Username = $"{account.Country}-{account.ID}".Trim().ToUpper();
                try
                {
                    using (var db = new DataContext())
                    {
                        db.LoginLogs.Add(new LoginLog
                        {
                            User = model.Username
                        });
                        await db.SaveChangesAsync();
                    }
                }
                catch
                {
                    //ignore
                }
                /// TO REMOVE
                //model.Username = "PH-214308";
                //model.Username = "PH-248813";
                var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.Failure:
                        throw new Exception("Account access has been disabled");
                    default:
                        throw new Exception("Access Denied");
                }
            }
            catch (Exception e)
            {
                var error = ExceptionHandler.GetMessages(e);
                ModelState.AddModelError("", error);
                return View("Login", model);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "App");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            return RedirectToAction("Index", "App");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}