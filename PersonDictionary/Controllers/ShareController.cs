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
            String userId1 = Request.Properties["UserId"].ToString();
            Note item;
            item = unitOfWork.Notations.Get(id, userId1);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [ApiAuthentAttr]
        public IHttpActionResult Post([FromBody] string msg)
        {
            if (msg == null)
            {
                return BadRequest();
            }
            try
            {
                String userId1 = Request.Properties["UserId"].ToString();
                var temp = new Note
                {
                    message = msg,
                    time = DateTime.Now,
                    PersonId = userId1
                };
                unitOfWork.Notations.Create(temp);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        [System.Web.Http.HttpDelete]
        [ApiAuthentAttr]
        [Route("api/Share/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            bool result = 
            unitOfWork.Notations.Delete(id);
            if (result)
            {
                unitOfWork.Save();
                return Ok();
            }
            else return NotFound();
        }
    }
}
