using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.App
{
    public class RouteConfigurator
    {
        private readonly RouteCollection routes;

        public RouteConfigurator(RouteCollection routes)
        {
            this.routes = routes;
        }

        public void Configure()
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                "MvcDefault",
                "{controller}/{action}/{id}",
                new { controller = "Root", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute("Default",
                 "",
                 new { controller = "Root", action = "Index" }
                 );

            routes.MapRoute("NotFound", "{*url}",
               new { controller = "Error", action = "Http404" });
        }
    }
}