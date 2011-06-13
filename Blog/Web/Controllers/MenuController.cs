using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Web.ViewModels;

namespace Blog.Web.Controllers
{
    public class MenuController : BaseController
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menu = new List<MenuViewModel>
            {
                new MenuViewModel { Selected = true, Title = "Blog", Url = Url.Action("index","Post")},
                new MenuViewModel { Selected = false, Title = "Admin", Url = Url.Action("index","Admin")},
            };
            return View(menu);
        }
    }
}
