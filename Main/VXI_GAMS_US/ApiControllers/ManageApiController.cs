using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VXI_GAMS_US.Controllers;
using VXI_GAMS_US.DATA.Data;
using VXI_GAMS_US.HELPER;
using VXI_GAMS_US.Models;
using VXI_GAMS_US.VIEWS.Entities;
using VXI_GAMS_US.VIEWS.View;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace VXI_GAMS_US.ApiControllers
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class ManageApiController : ApiController
    {
        [HttpPost]
        [Route("api/ManageApi/{id}/test2")]
        public async Task<IHttpActionResult> Test2(Guid id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = await db.Database.SqlQuery<AspRole>("SELECT Id, Name FROM dbo.AspNetRoles").ToListAsync();
                    return Ok(entity.OrderBy(x => x.Name));
                }
            }
            catch (Exception e)
            {
                var result = ExceptionHandler.GetMessages(e);
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("api/ManageApi/{id}/test")]
        public HttpResponseMessage Test(Guid id)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count < 1)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            foreach(string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                if (postedFile == null) continue;
                var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                postedFile.SaveAs(filePath);
                // NOTE: To store in memory use postedFile.InputStream
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        public static async Task<string> SetUserRole(RegisterViewModel account, string role)
        {
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
                var vm = new LoginViewModel
                {
                    Username = account.Username
                };
            Check:
                var user = await userManager.FindByNameAsync(vm.Username);
                if (user == null)
                {
                    //throw new Exception($"Cannot find user: {account.Hrid}");
                    var userResult = await userManager.CreateAsync(
                        new ApplicationUser()
                        {
                            UserName = vm.Username,
                            Email = vm.Email
                        }, vm.Password);
                    if (!userResult.Succeeded)
                    {
                        //throw new ApplicationException($"Creating User failed with error(s): {userResult.Errors}");
                        throw new ApplicationException($"Assigning User failed.");
                    }
                    goto Check;
                }
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                await userManager.UpdateAsync(user);
                await userManager.AddToRoleAsync(user.Id, role);
                return user.Id ?? string.Empty;
            }
        }
        
        public static async Task<ApplicationUser> GetUser(string userId)
        {
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
                return await userManager.FindByIdAsync(userId);
            }
        }
    }
}