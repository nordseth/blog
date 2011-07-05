using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Web.App.AutoMapper;
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
            //filters.Add(new RavenActionFilterAttribute());
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
            HandleAppError();
        }

        protected void Application_EndRequest()
        {
            //var dbSession = (Raven.Client.IDocumentSession)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(Raven.Client.IDocumentSession));
            //dbSession.SaveChanges();

            //new StructureMap.Pipeline.HybridLifecycle().FindCache().DisposeAndClear();
        }

        private void HandleAppError()
        {
            var error = Server.GetLastError();

            IController errorController = DependencyResolver.Current.GetService<Blog.Web.Controllers.ErrorController>();
            var errorUrl = Context.Request.Url.OriginalString;
            var errorId = GetErrorId();

            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Http500");
            errorRoute.Values.Add("errorUrl", errorUrl);
            errorRoute.Values.Add("exception", error);
            errorRoute.Values.Add("errorId", errorId);

            RouteData routeData = GetRouteData();
            errorRoute.Values.Add("errorRouteData", routeData);

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

        void WSFederationAuthenticationModule_SessionSecurityTokenCreated(object sender, Microsoft.IdentityModel.Web.SessionSecurityTokenCreatedEventArgs e)
        {
            Microsoft.IdentityModel.Web.FederatedAuthentication.SessionAuthenticationModule.IsSessionMode = true;
        }

        private string GetErrorId()
        {
            string errorId = Context.Items[MagicKeys.ErrorId.FullName()] as string;
            if (errorId == null)
            {
                errorId = Guid.NewGuid().ToString();
                Context.Items[MagicKeys.ErrorId.FullName()] = errorId;
            }
            return errorId;
        }

        private RouteData GetRouteData()
        {
            var handler = Context.Handler as MvcHandler;
            if (handler != null)
            {
                return handler.RequestContext.RouteData;
            }
            else
            {
                return null;
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