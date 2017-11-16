using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonDictionary.Models;
using PersonDictionary.SendingToEmail;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net.Http;

namespace PersonDictionary.Controllers
{
    public class HomeController : Controller
    {
        private DataContext dbPersons;
        private static string AuthorizeStatus;
        private static string Id_Trace;
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
            ViewBag.AuthorizeStatus = AuthorizeStatus;
            ViewBag.Id_Trace = Id_Trace;
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
                        ModelState.AddModelError("", "Login or password is wrong.");
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
                               controllerName: "Account");

                        return Redirect(returnUrl);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }
            }
            ViewBag.returnUrl = returnUrl;
            return new HttpStatusCodeResult(404);
        }
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return Redirect("/Home/Initial");
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
                        Email = model.eMail,
                        UserName = model.Name,
                        login = model.login,
                        password = model.password
                    },
                    model.password);
                if (result.Succeeded)
                {
                    try
                    {
                        if (dbPersons == null) dbPersons = new DataContext();
                        //model.Id; get !
                        var sql_query = dbPersons.Database
                            .SqlQuery<String>(String
                            .Format("SELECT Id FROM AspNetUsers WHERE Email = '{0}' AND password='{1}'",
                            model.eMail, model.password));
                        model.Id = sql_query.Single();
                        Id_Trace = model.Id;
                        dbPersons.Persons.Add(model);
                        dbPersons.SaveChanges();
                        dbPersons.Dispose();
                        AuthorizeStatus = "you was successfully registered ";
                        return Redirect("/Home/Initial");
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var validationErrors in e.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        AuthorizeStatus = "Some exception was arisen";
                    }
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    AuthorizeStatus = "the model is wrong";
                }
            }
            return RedirectToAction("Initial");
        }
        public ActionResult RememberPassword()
        {
            return PartialView();
        }
        public ActionResult SendPasswordToEmail(String adress)
        {
            bool SendingResult = true;
            try
            {
                using (DataContext dbContext = new DataContext())
                {
                    var passwordAsQuery = dbContext.Database
                        .SqlQuery<String>(String
                        .Format("SELECT password FROM AspNetUsers WHERE Email = '{0}'",
                        adress));
                    String password = passwordAsQuery.Single();
                    SendingResult = SendingToEmail.
                    SendPasswordToEmail.SendPassword(adress, password);
                }
            }
            catch (Exception)
            {
                SendingResult = false;
            } 
            if (SendingResult == true)
                return Content("Your password was sent successfully");
            else
                return Content("Error of sending Your password");
        }
    }
}