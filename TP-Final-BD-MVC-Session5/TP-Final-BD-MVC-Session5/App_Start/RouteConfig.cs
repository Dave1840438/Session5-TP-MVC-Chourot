using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TP_Final_BD_MVC_Session5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ESports", action = "Index", id = UrlParameter.Optional }
            );

            String dbPath = Path.Combine(HttpContext.Current.Server.MapPath(@"App_Data"), "MainDB.mdf");
            TP_Final_BD_MVC_Session5.Constants.ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename='" + dbPath +  "';Integrated Security=True;Connect Timeout=10";  
        }
    }
}
