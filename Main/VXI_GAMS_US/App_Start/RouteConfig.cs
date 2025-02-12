﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VXI_GAMS_US
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreWindowsLoginRoute();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "AppOverride",
            //    url: "App/{*.}",
            //    defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
            //);
            //routes.AppendTrailingSlash = true;
        }
    }
}
