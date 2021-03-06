﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PersonDictionary
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "",
                url: "Account/Index/{id}/{pageNumber}/{pageSize}",
                defaults: new
                {
                    controller = "Account",
                    action = "Index",
                    id = UrlParameter.Optional,
                    pageNumber = UrlParameter.Optional,
                    pageSize = UrlParameter.Optional
                }
            );

            routes.MapRoute(
               name: "",
               url: "Account/GetNotesOnPage/{page}/{quantityOnPage}",
               defaults: new {
                   controller = "Account",
                   action = "GetNotesOnPage",
                   page = UrlParameter.Optional,
                   quantityOnPage = UrlParameter.Optional
               }
           );

           routes.MapRoute(
              name: "",
              url: "Account/DownloadFoto/{uploadFile}",
              defaults: new
              {
                  controller = "Account",
                  action = "DownloadFoto",
                  uploadFile = UrlParameter.Optional
              }
          );
            routes.MapRoute(
              name: "",
              url: "Account/DelNote/{id}",
              defaults: new
              {
                  controller = "Account",
                  action = "DelNote",
                  id = UrlParameter.Optional
              }
          );

            routes.MapRoute(
                name: "Def",
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Initial"
                }
            );
            routes.MapRoute(
                name: "",
                url: "Home/SendPasswordToEmail/{adress}",
                defaults: new
                {
                    controller = "Home",
                    action = "SendPasswordToEmail",
                    adress = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Home",
                    action = "Initial",

                }
            );
        }
    }
}
