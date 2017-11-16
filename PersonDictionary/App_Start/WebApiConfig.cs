using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PersonDictionary
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/Share/GetNotes",
                defaults: new
                {
                    controller = "Share",
                    action = "GetNotes"
                }
            );
            config.Routes.MapHttpRoute(
                name: "LoginIn",
                routeTemplate: "Home/Login/{model}/{returnUrl}",
                defaults: new
                {
                    controller = "Home",
                    action = "Login",
                    model = RouteParameter.Optional,
                
                }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
