using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using PersonDictionary.Models;
using PersonDictionary.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace PersonDictionary.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        private UnitOfWork unitOfWork;
    
        public ActionResult Index(int pageNumber =1 , int pageSize = 5)
        {
            if (!User.Identity.IsAuthenticated) throw new Exception(); 
            string id = User.Identity.GetUserId();
           
            Session["userId"] = id;

            var model = new NotesViewModel();
            
            var person = unitOfWork.Persons.Get(Session["userId"].ToString());
            if (person != null)
            {
                person.Notes = unitOfWork.Notations.GetAll(Session["userId"].ToString()).ToList();
                model.Person = person; 
                model.PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = person.Notes.Count()
                };
                return View(model);
            }
            else 
            return  new HttpStatusCodeResult(500);               
        }
        public ActionResult LogOut()
        {
            return RedirectToAction(actionName:"Initial", controllerName:"Home");
        }
        // methods notes manage
        public AccountController()
        {
            unitOfWork = new UnitOfWork();
        }
        [HttpPost]
        public ActionResult AddNote(String newNote)
        {
            if (!String.IsNullOrEmpty(newNote))
            {
                Note noteToWrite = new Note
                {
                    message = newNote,
                    PersonId = Session["userId"].ToString(),
                    time = DateTime.Now,
                };
                unitOfWork.Notations.Create(noteToWrite);
                unitOfWork.Save();
                return Redirect("/Account/Index");
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
        
        public ActionResult DelNote(int id)
        {
            try
            {
                unitOfWork.Notations.Delete(id);
                unitOfWork.Save();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return Redirect("/Account/Index");
        }
        
        // methods person info manage
        [HttpPost]
        public ActionResult DownloadFoto(HttpPostedFileBase uploadFile)
        {
            try
            {
                if (ModelState.IsValid && uploadFile.ContentLength > 0)
                {                     
                    var alterPerson = unitOfWork.Persons.Get(Session["userId"].ToString()); 
                    MemoryStream target = new MemoryStream();
                    uploadFile.InputStream.CopyTo(target);
                    alterPerson.PasswordConfirm = alterPerson.password;
                    alterPerson.Foto = target.ToArray();
                    unitOfWork.Persons.Update(alterPerson);
                    unitOfWork.Save();
                }
                
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
            catch
            {
                
            }
            return Redirect("/Account/Index");
        }
    }
}