using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Web.App.AutoMapper;
using Blog.Web.App.Filters;
using Blog.Web.App;
using Blog.Core;

namespace Blog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();
            RegisterHttpModules(this);

        }

        public static void RegisterHttpModules(HttpApplication context)
        {
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new RavenActionFilterAttribute());
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            new RouteConfigurator(RouteTable.Routes).Configure();
            AutoMapperConfiguration.Configure();

            this.Log().Info("Application_Start");
        }

        protected void Application_End()
        {
            this.Log().Info("Application_End");
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();

            var handler = Context.Handler as MvcHandler;
            if (handler != null)
            {
                IController errorController = DependencyResolver.Current.GetService<Blog.Web.Controllers.ErrorController>();
                var errorUrl = Context.Request.Url.OriginalString;
                var routeData = handler.RequestContext.RouteData;
                var errorId = Guid.NewGuid().ToString();

                var errorRoute = new RouteData();
                errorRoute.Values.Add("controller", "Error");
                errorRoute.Values.Add("action", "Http500");
                errorRoute.Values.Add("errorUrl", errorUrl);
                errorRoute.Values.Add("errorRouteData", routeData);
                errorRoute.Values.Add("exception", error);
                errorRoute.Values.Add("errorId", errorId);

                this.Log().ErrorFormat("Handeling error with id {1}{0}url {2}{0}route {3}{0}{4}",
                    Environment.NewLine,
                    errorId,
                    errorUrl,
                    ErrorString(routeData),
                    error
                    );

                try
                {
                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), errorRoute));
                    Server.ClearError();
                }
                catch
                {
                    this.Log().ErrorFormat("Failed handeling error with id {0}", errorId);
                }
            }
            else
            {
                this.Log().Error("Not handeling error", error);
            }
        }

        private string ErrorString(RouteValueDictionary values)
        {
            if (values != null)
                return values.ToStringList();
            else
                return "(null)";
        }

        private string ErrorString(RouteData routeData)
        {
            if (routeData != null)
                return string.Format("{{values : {0}, tokens : {1}}}",
                    ErrorString(routeData.Values),
                    ErrorString(routeData.DataTokens));
            else
                return "(null)";
        }
    }
}