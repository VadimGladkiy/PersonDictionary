using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Net;
using Microsoft.AspNet.Identity;

namespace PersonDictionary.ApiService
{
    public class ApiAuthentAttr : AuthorizationFilterAttribute 
    {
        private String userId;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                userId = actionContext.RequestContext.Principal.Identity.GetUserId();
                actionContext.Request.Properties.Add("UserId", userId);
            }
            else
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string autorizationToken = actionContext.Request
                    .Headers.Authorization.Parameter;
                string decodedAuthenToken = Encoding.UTF8
                    .GetString(Convert.FromBase64String(autorizationToken));

                string[] userNamePassArray = decodedAuthenToken.Split(':');
                string userName = userNamePassArray[0];
                string password = userNamePassArray[1];

                String userId = ApiLogin.Login(userName, password);
                if (userId != null)
                {
                    actionContext.Request.Properties.Add("UserId", userId);
                    IPrincipal principal = new GenericPrincipal(
                        new GenericIdentity(userName), roles: new string[] { password });
                    
                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;
                }
                else
                {
                    actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}