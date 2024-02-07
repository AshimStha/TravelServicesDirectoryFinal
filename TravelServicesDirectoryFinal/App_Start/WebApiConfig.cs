using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TravelServicesDirectoryFinal
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                // added an {action} componenet for the URL
                // The {action} is included for the route URL paths. GET: api/CustomersData/FindCustomer/5
                // Here, action is FindCustomer
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
