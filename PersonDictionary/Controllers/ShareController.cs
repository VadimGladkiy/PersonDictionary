using Microsoft.Owin.Security;
using PersonDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Diagnostics;

namespace PersonDictionary.Controllers
{
    [Authorize]
    public class ShareController : ApiController
    {
        public ShareController()
        {
           
        }
        [HttpGet]
        public IHttpActionResult GetNotes()
        {
            IEnumerable<Note> items;
            using (DataContext dbContext = new DataContext())
            {
                items = dbContext.Notations.ToList()
                    .OrderBy(x => x.time.Second);
            }
            return Ok(items);
        }
        [HttpGet]
        public IHttpActionResult GetTheOne(int id)
        {
            Note item;
            using (DataContext dbContext = new DataContext())
            {
                item = dbContext.Notations.FirstOrDefault(x => x.id == id);
            }
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public IHttpActionResult Post([FromBody]Note newInstance)
        {
            if (!ModelState.IsValid || newInstance == null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (DataContext dbContext = new DataContext())
                {
                    var userByName = dbContext.Persons
                        .FirstOrDefault(x => x.login == User.Identity.Name);
                    var id = userByName.Id;

                    var temp = new Note
                    {
                        message = newInstance.message,
                        time = DateTime.Now,
                        PersonId = id
                    };
                    dbContext.Notations.Add(temp);
                    dbContext.SaveChanges();
                }
            String location = Request.RequestUri + "/" + newInstance.PersonId.ToString();
            return Created(location, newInstance);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool result;
            using (var dbContext = new DataContext())
            {
                var note = dbContext.Notations
                    .FirstOrDefault(x => x.id == id);
                if (note != null)
                {
                    dbContext.Notations.Remove(note);
                    result = true;
                }
                else result = false;
            }
            if (result)
            {
                return Ok();
            }
            else { return NotFound(); }
        }
        public IHttpActionResult Put(int id, Note updated)
        {
            try
            {
                using (var dbContext = new DataContext())
                {
                    var obj = dbContext.Notations.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        obj.message = updated.message;
                        obj.time = DateTime.Now;
                        dbContext.SaveChanges();
                        return Ok();
                    }
                    else return NotFound();
                }
            }
            catch
            {
                return BadRequest(); 
            }
        }
    }
}
