using PersonDictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PersonDictionary.Filters;
using PersonDictionary.ApiService;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace PersonDictionary.Controllers
{
    
    public class ShareController : ApiController
    {
        private UnitOfWork unitOfWork;
        public ShareController()
        {
            unitOfWork = new UnitOfWork();

            var user_id = RequestContext.Principal.Identity.GetUserId();

            if (user_id == null)
            {
                // some handler
            }
            else
            unitOfWork.currUserId = user_id;
        }
        [LogActionFilter]
        [ApiAuthentAttr]
        public IHttpActionResult GetNotes()
        {
            IEnumerable<Note> items;
            String userName1 =  Thread.CurrentPrincipal.Identity.Name;
            String userId1  = Request.Properties["UserId"].ToString();
            items = unitOfWork.Notations.GetAll(userId1)
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
