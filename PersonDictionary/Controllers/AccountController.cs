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
                using (DataContext dbContext = new DataContext())
                {
                    dbContext.Notations.Add(noteToWrite);
                    dbContext.SaveChanges();
                }                
            }
            return new EmptyResult();
        }
        [HttpDelete]
        public ActionResult DelNote(int id)
        {
            using (DataContext dbContext = new DataContext())
            {
                try
                {
                    var itemToDel = dbContext.Notations.First(x => x.id == id);

                    TimeSpan delta = new TimeSpan(24,0,0);
                    if ((DateTime.Now.Second - itemToDel.time.Second) > delta.Seconds)
                        throw new Exception();

                    dbContext.Notations.Remove(itemToDel);
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    // some handler 
                }
            }
            return RedirectToAction("Index", new { id = Session["userId"]});
        }
        [HttpGet]
        public ActionResult GetNotesOnPage(int page, int quantityOnPage = 5)
        {
            IEnumerable<Note> items;
            using (DataContext dbContext = new DataContext())
            {
                items = dbContext.Notations.ToList()
                    .OrderBy(x => x.time.Second)
                    .Skip(quantityOnPage * (page-1))
                    .Take(quantityOnPage);
            }
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
                    using (DataContext dbContext = new DataContext())
                    {
                        var alterPerson = dbContext.Persons
                            .FirstOrDefault(x => x.Id == id);
                        MemoryStream target = new MemoryStream();
                        uploadFile.InputStream.CopyTo(target);
                        alterPerson.PasswordConfirm = alterPerson.password;
                        alterPerson.Foto = target.ToArray();
                        dbContext.SaveChanges();
                    }
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
        public ActionResult SendMsgs()
        {
            IEnumerable<Note> items;
            using (DataContext dbContext = new DataContext())
            {
                items = dbContext.Notations.ToList()
                    .OrderBy(x => x.time.Second)                    
                    .Take(5);
            }
            TelegramMessageSender sender = new TelegramMessageSender();
            foreach (var ptr in items)
            {
                sender.sendMessage("+380*********", ptr.message);
            } 

            return RedirectToAction("Index", new { id = Session["userId"] });
        }
    
    }
}