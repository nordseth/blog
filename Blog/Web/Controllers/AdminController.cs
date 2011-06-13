using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Model;
using Blog.Web.ViewModels;

namespace Blog.Web.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Config()
        {
            var blogConfig = Session.Load<BlogConfig>("Blog/Config");
            if (blogConfig == null)
            {
                blogConfig = new BlogConfig();
                blogConfig.Id = "Blog/Config";
                Session.Store(blogConfig);
            }
            return View(blogConfig.MapTo<BlogConfigViewModel>());
        }

        [HttpPost]
        [ActionName("Config")]
        public ActionResult ConfigUpdate(BlogConfigViewModel config)
        {

            var blogConfig = config.MapTo<BlogConfig>();
            blogConfig.Id = "Blog/Config";
            Session.Store(blogConfig);

            AddMessage("blog config saved");

            return RedirectToAction("Config");
        }
    }
}
