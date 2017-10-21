using Owin;
using Microsoft.Owin;
using PersonDictionary.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(PersonDictionary.App_Start.Startup))]
namespace PersonDictionary.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // настраиваем контекст и менеджер
            app.CreatePerOwinContext<MyIdentityContext>(MyIdentityContext.Create);
            app.CreatePerOwinContext<CustomerManager>(CustomerManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}