using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Controllers
{
    public class PostController : BaseController
    {
        public ActionResult Index()
        {
            return this.InvokeHttp404(HttpContext);
        }

    }
}
