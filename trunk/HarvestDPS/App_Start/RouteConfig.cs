using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HarvestDPS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TimeDay",
                url: "time/{action}/{year}/{month}/{day}",
                defaults: new { controller = "Time", action = "Day" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Time", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
