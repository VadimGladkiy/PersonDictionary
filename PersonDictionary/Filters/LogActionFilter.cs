using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace PersonDictionary.Filters
{
    public class LogActionFilter : System.Web.Http.Filters.ActionFilterAttribute
    {

        public override Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            Log("OnActionExecuting")
            );
            
        }
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            Log("OnActionExecuted")
            );

        }

        private void Log(string methodName)
        {
            
            Debug.WriteLine("Action Filter Log: "+ methodName);
        }

        public LogActionFilter()
        {
                
        }
    }
}