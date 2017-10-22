using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonDictionary.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace PersonDictionary.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        private UnitOfWork unitOfWork;
        // [Authorize]
        public ActionResult Index(int id)
        {
            Session["userId"] = id;
            using (var dbContext = new DataContext())
            {
                var person = dbContext.Persons.FirstOrDefault(x => x.Id == id);
                if (person != null)
                    person.Notes = dbContext.Notations
                        .Where(x => x.PersonId == id).ToList();
                ViewData["notesCount"] = dbContext.Notations
                    .Where(x => x.PersonId == id).Count();
                return View(person);
            } 
        }
        public ActionResult LogOut()
        {
            return RedirectToAction(actionName:"LogOut", controllerName:"Home");
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
                    PersonId = (int)Session["userId"],
                    time = DateTime.Now,
                };
                unitOfWork.Notations.Create(noteToWrite);
                unitOfWork.Save();
                return View("GetNotesOnPage", noteToWrite);
            }
            return new EmptyResult();
        }
        [HttpDelete]
        public ActionResult DelNote(int id)
        {
            unitOfWork.Notations.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index", new { id = Session["userId"]});
        }
        [HttpGet]
        public ActionResult GetNotesOnPage(int page, int quantityOnPage = 5)
        {
            IEnumerable<Note> items;
            items = unitOfWork.Notations.GetAll((int)Session["userId"])
                    .OrderBy(x => x.time.Second)
                    .Skip(quantityOnPage * (page-1))
                    .Take(quantityOnPage);
            
            return PartialView(items);
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
            return RedirectToAction("Index", routeValues: new { id = id });
        }
    }
}