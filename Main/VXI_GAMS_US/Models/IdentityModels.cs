using System;
using System.Configuration;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VXI_GAMS_US.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            //Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            Database.SetInitializer<ApplicationDbContext>(new ApplicationInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        private static readonly string[] DefaultRoles = new[] { "Super Administrator", "Administrator", "User" };

        protected override void Seed(ApplicationDbContext context)
        {
            using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
            {
                foreach (var role in DefaultRoles)
                {
                    if (roleManager.RoleExists(role))
                        continue;
                    roleManager.Create(new IdentityRole(role));
                }
            }
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
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
                var superAdmin = ConfigurationManager.AppSettings["ITECH_USERS"].Trim();
                foreach (var hrid in superAdmin.Split(','))
                {
                    var model = new LoginViewModel
                    {
                        Username = $"PH-{hrid}".Trim().ToUpper()
                    };
                    var userResult = userManager.Create(new ApplicationUser { UserName = model.Username, Email = model.Email }, model.Password);
                    if (!userResult.Succeeded)
                    {
                        return;//throw new ApplicationException($"Creating User failed with error(s): {userResult.Errors}");
                    }
                    //userManager.AddToRole(SuperAdmin, "Administrator");
                    var user = userManager.FindByName(model.Username);
                    if (user == null)
                        return;//throw new Exception($"Cannot find user: {model.Username}");
                    user.EmailConfirmed = true;
                    userManager.Update(user);
                    userManager.AddToRole(user.Id, DefaultRoles[0]);
                }
            }
        }
    }
}