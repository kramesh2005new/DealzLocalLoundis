using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IdealMall.Helpers;
using IdealMall.BusinessAccess;
using System.Configuration;

namespace IdealMall
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "IdealMall.Controllers" } // Namespace of controllers in root area
            );
        }

        

        protected void Application_Start()
        {
            Database.ConnectionString = ConfigurationManager.ConnectionStrings["Dev"].ConnectionString;
            AreaRegistration.RegisterAllAreas();

            // Using "order" value 1 means it will run after unordered filters
            // associated with specific controllers or actions
            GlobalFilters.Filters.Add(new RedirectMobileDevicesToMobileAreaAttribute(), 1);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}