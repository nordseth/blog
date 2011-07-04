using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Blog.Model;
using Blog.Web.ViewModels;
using System.Web.Routing;

namespace Blog.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public Blog.Dal.BlogConfigRepo BlogConfigRepo { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var blogConfig = BlogConfigRepo.GetConfig();
                ViewBag.BlogConfig = blogConfig.MapTo<BlogConfigViewModel>();
            }
        }

        protected void AddMessage(object message)
        {
            TempData["Message"] = message;
        }

        #region Http404 handling
        // http://stackoverflow.com/questions/619895/how-can-i-properly-handle-404s-in-asp-net-mvc/2577095#2577095

        protected override void HandleUnknownAction(string actionName)
        {
            // original comment: If controller is ErrorController dont 'nest' exceptions
            // personaly i like nested exception
            //if (this.GetType() != typeof(ErrorController))
            this.InvokeHttp404(HttpContext);
        }

        public ActionResult InvokeHttp404(HttpContextBase httpContext)
        {
            IController errorController = DependencyResolver.Current.GetService<ErrorController>();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Http404");
            errorRoute.Values.Add("url", httpContext.Request.Url.OriginalString);
            errorController.Execute(new RequestContext(httpContext, errorRoute));

            return new EmptyResult();
        }

        #endregion
    }
}
