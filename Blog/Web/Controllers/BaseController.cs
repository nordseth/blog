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
        public IDocumentSession DbSession { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            var blogConfig = DbSession.Load<BlogConfig>("Blog/Config");
            if (blogConfig == null)
            {
                blogConfig = new BlogConfig();
                blogConfig.Id = "Blog/Config";
                DbSession.Store(blogConfig);
            }
            ViewBag.BlogConfig = blogConfig.MapTo<BlogConfigViewModel>();

            DbSession.SaveChanges();
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
