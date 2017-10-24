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
        public ActionResult GetNotesOnPage(int pageNumber, int pageSize = 5)
        {
            var model = new NotesViewModel();
            
            model.Notes = unitOfWork.Notations.GetAll((int)Session["userId"])
                    .OrderBy(x => x.time)
                    .Skip(pageSize * (pageNumber-1))
                    .Take(pageSize);
            model.PageInfo = new PageInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = model.Notes.Count()
            };

            return PartialView(model);
        }
        public ActionResult GetAllNotes()
        {
            IEnumerable<Note> items;
            items = unitOfWork.Notations.GetAll((int)Session["userId"])
                    .OrderBy(x => x.time);
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