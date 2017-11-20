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
using System.Text;

namespace PersonDictionary.Controllers
{
    
    public class ShareController : ApiController
    {
        private static UnitOfWork unitOfWork;
        static ShareController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ShareController()
        {
            
        }
        [HttpGet]
        [LogActionFilter]
        [ApiAuthentAttr]
        public IHttpActionResult GetNotes()
        {
            IEnumerable<Note> items;
            String userName1 =  Thread.CurrentPrincipal.Identity.Name;
            String userId1  = Request.Properties["UserId"].ToString();
            items = unitOfWork.Notations.GetAll(userId1).ToList();
            return Ok(items);            
        }

        [HttpGet]
        [ApiAuthentAttr]
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
        [ApiAuthentAttr]
        public IHttpActionResult Post([FromBody]String newMessage)
        {
            if ( newMessage == null)
            {
                return BadRequest();
            }
            String userId1 = Request.Properties["UserId"].ToString();
            var temp = new Note
            {
                message = newMessage,
                time = DateTime.Now,
                PersonId = userId1
            };
            unitOfWork.Notations.Create(temp);
            return Ok();           
        }

        
        [System.Web.Http.HttpDelete]
        [ApiAuthentAttr]
        [Route("api/Share/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            bool result = 
            unitOfWork.Notations.Delete(id);
            if (result) return Ok();
            else return NotFound();
        }
    }
}
