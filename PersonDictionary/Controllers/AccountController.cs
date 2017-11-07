using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonDictionary.Models;
using PersonDictionary.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace PersonDictionary.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        private UnitOfWork unitOfWork;
        // [Authorize]
        public ActionResult Index(int id, int pageNumber =1 , int pageSize = 5)
        {
            Session["userId"] = id;
            var model = new NotesViewModel();
            
            var person = unitOfWork.Persons.Get(id);
            if (person != null)
            {
                person.Notes = unitOfWork.Notations.GetAll(id).ToList();
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
                int id = (int)Session["userId"];
                Note noteToWrite = new Note
                {
                    message = newNote,
                    PersonId = id,
                    time = DateTime.Now,
                };
                unitOfWork.Notations.Create(noteToWrite);
                unitOfWork.Save();
                return Redirect("/Account/Index/"+ id);
            }
            return new EmptyResult();
        }
        
        public ActionResult DelNote(int id)
        {
            unitOfWork.Notations.Delete(id);
            unitOfWork.Save();
            return Redirect("/Account/Index/"+(int)Session["userId"]);
        }
        
        // methods person info manage
        [HttpPost]
        public ActionResult DownloadFoto(HttpPostedFileBase uploadFile)
        {
            int id = (int)Session["userId"];
            try
            {
                if (ModelState.IsValid && uploadFile.ContentLength > 0)
                {                     
                    var alterPerson = unitOfWork.Persons.Get(id); 
                    MemoryStream target = new MemoryStream();
                    uploadFile.InputStream.CopyTo(target);
                    alterPerson.PasswordConfirm = alterPerson.password;
                    alterPerson.Foto = target.ToArray();
                    unitOfWork.Persons.Update(alterPerson);
                    unitOfWork.Save();
                }
                ViewData["message"] = "Upload successful";
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
                ViewData["message"] = "Upload failed";
            }
            return Redirect("/Account/Index/"+ id );
        }
    }
}