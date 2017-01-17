using System.Web.Routing;

namespace IdealMall.Helpers
{
    public class RedirectMobileDevicesToMobileAboutPageAttribute : RedirectMobileDevicesToMobileAreaAttribute
    {
        protected override RouteValueDictionary GetRedirectionRouteValues(RequestContext requestContext)
        {
            // By overriding this method, you can extract any relevant parameters from the incoming request
            // (e.g., the ID of an item) and construct the route values for the mobile equivalent URL
            var routeValues = base.GetRedirectionRouteValues(requestContext);
            routeValues["action"] = "About";
            return routeValues;
        }
    }
}