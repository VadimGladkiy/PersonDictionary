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
using Microsoft.AspNet.Identity;

namespace PersonDictionary.Controllers
{
    [Authorize]
    public class ShareController : ApiController
    {
        UnitOfWork unitOfWork;
        public ShareController()
        {
            unitOfWork = new UnitOfWork();
            var user_id = RequestContext.Principal.Identity.GetUserId();
            int number;
            bool result = Int32.TryParse(user_id, out number);
            if (result == true)
            {
                unitOfWork.currUserId = number;
            }else
            {
                throw new NotImplementedException();
            }        
        }
        [HttpGet]
        public IHttpActionResult GetNotes()
        {
            IEnumerable<Note> items;
            items = unitOfWork.Notations.GetAll(unitOfWork.currUserId)
            .ToList().OrderBy(x => x.time);
            return Ok(items);            
        }
        [HttpGet]
        public IHttpActionResult GetTheOne(int id)
        {
            Note item;
            item = unitOfWork.Notations.Get(id);
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
            var temp = new Note
            {
                message = newInstance.message,
                time = DateTime.Now,
                PersonId = unitOfWork.currUserId
            };
            unitOfWork.Notations.Create(temp);
                    
            String location = Request.RequestUri + "/" + newInstance.PersonId.ToString();
            return Created(location, newInstance);           
        }
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool result = 
            unitOfWork.Notations.Delete(id);
            if (result) return Ok();
            else return NotFound();
        }
        public IHttpActionResult Put(Note updated)
        {
            if (updated != null || !ModelState.IsValid)
            {
                var obj = new Note
                {
                    message = updated.message,
                    time = DateTime.Now
                };
                unitOfWork.Notations.Update(updated);
            return Ok();
            }
            else return NotFound();           
        }
    }
}
