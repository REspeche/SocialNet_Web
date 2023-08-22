using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialNet_Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Improve SEO by stopping duplicate URL's due to case differences or trailing slashes.
            // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;

            // IgnoreRoute - Tell the routing system to ignore certain routes for better performance.
            // Ignore .axd files.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // Ignore everything in the Content folder.
            routes.IgnoreRoute("css/{*pathInfo}");
            // Ignore everything in the Scripts folder.
            routes.IgnoreRoute("js/{*pathInfo}");
            // Ignore the humans.txt file.
            routes.IgnoreRoute("humans.txt");

            // Enable attribute routing.
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "LocalisationUsers",
                url: "{lang}/{usercode}/{controller}/{action}/{id}",
                defaults: new { controller = "Wall", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"[a-z]{2}|[a-z]{2}-[a-zA-Z]{2}", usercode = @"^\d{20}$", controller = @"^Common$|^Wall$|^Photo$|^Video$|^Friend$|^Profile$|^Group$|^Company" }
            );

            routes.MapRoute(
                name: "Users",
                url: "{usercode}/{controller}/{action}/{id}",
                defaults: new { controller = "Wall", action = "SwitchByName", id = UrlParameter.Optional },
                constraints: new { usercode = @"^\d{20}$", controller = @"^Common$|^Wall$|^Photo$|^Video$|^Friend$|^Profile$|^Group$|^Company" }
            );

            routes.MapRoute(
                name: "Localisation",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Wall", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"[a-z]{2}|[a-z]{2}-[a-zA-Z]{2}" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Wall", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
