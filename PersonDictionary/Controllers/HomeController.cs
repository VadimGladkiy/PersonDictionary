using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonDictionary.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace PersonDictionary.Controllers
{
    public class HomeController : Controller
    {
        private DataContext dbPersons;

        private CustomerManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<CustomerManager>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Initial()
        {
            return View();
        }

        // authorize
        public ActionResult GetFormLogin()
        {
            return PartialView();
        }
        public async Task<ActionResult> Login(Customer model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (dbPersons == null) dbPersons = new DataContext();
                    Person person = dbPersons.Persons
                        .FirstOrDefault(x => x.login == model.login || x.password == model.password);
                    Customer user = await UserManager.FindAsync(person.Name, model.password);
                    
                    dbPersons.Dispose();
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль.");
                    }
                    else
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction(actionName: "Index",
                               controllerName: "Account", routeValues: new { id = person.Id });

                        return Redirect(returnUrl);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }
            }
            ViewBag.returnUrl = returnUrl;
            return RedirectToAction("GetFormLogin");
        }
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("GetFormLogin");
        }
        
        // registration
        [HttpGet]
        public ActionResult GetFormRegistration()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<ActionResult> UserRegister(Person model)
        {
            if (ModelState.IsValid)
            {
                Person newUser = new Person
                {
                    Name = model.Name,
                    eMail = model.eMail,
                    password = model.password,
                    login = model.login,
                   
                };

                IdentityResult result = await UserManager
                    .CreateAsync(new Customer {
                        
                        UserName = model.Name,
                        login = model.login,
                        password = model.password
                    },
                    model.password);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetFormLogin", "Home");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return RedirectToAction("GetFormRegistration");
        }
    }
}