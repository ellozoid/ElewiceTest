using DBModel.Helpers;
using DBModel.Models.Identity;
using ElewiceTest;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using DBModel.Managers;

[assembly: OwinStartup(typeof(Startup))]

namespace ElewiceTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            UserRepositoryManager userManager = new UserRepositoryManager();
            app.CreatePerOwinContext(() => new UserManager(userManager.Users));
            app.CreatePerOwinContext<SignInManager>((options, context) => new SignInManager(context.GetUserManager<UserManager>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}