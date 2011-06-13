using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Blog.Web.Controllers
{
    public class ErrorController : BaseController
    {
        #region Http404
        // http://stackoverflow.com/questions/619895/how-can-i-properly-handle-404s-in-asp-net-mvc/2577095#2577095

        public ActionResult Http404(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            var model = new NotFoundViewModel();
            // If the url is relative ('NotFound' route) then replace with Requested path
            model.RequestedUrl = Request.Url.OriginalString.Contains(url) & Request.Url.OriginalString != url ?
                Request.Url.OriginalString : url;
            // Dont get the user stuck in a 'retry loop' by
            // allowing the Referrer to be the same as the Request
            model.ReferrerUrl = Request.UrlReferrer != null &&
                Request.UrlReferrer.OriginalString != model.RequestedUrl ?
                Request.UrlReferrer.OriginalString : null;

            // TODO: insert ILogger here

            return View("NotFound", model);
        }
        public class NotFoundViewModel
        {
            public string RequestedUrl { get; set; }
            public string ReferrerUrl { get; set; }
        }

        #endregion

        public ActionResult Http500(string errorUrl, System.Web.Routing.RouteData errorRouteData, Exception exception, Guid errorId)
        {
            if (errorId != Guid.Empty)
                ViewBag.ErrorId = errorId;
            return View("Error");
        }

        public ActionResult TestHttp500()
        {
            throw new Exception("Http500");
        }
    }
}
