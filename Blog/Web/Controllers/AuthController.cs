using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Model;
using Blog.Core;
using Microsoft.IdentityModel.Claims;
using System.Text;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Web;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Configuration;

namespace Blog.Web.Controllers
{
    public class AuthController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.HrdFeed = AppConfigWrapper.HrdFeed;
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Token(string wresult)
        {
            var userData = new UserData();
            userData.Options.Add("wresult", wresult);
            Session.Store(userData);

            AddMessage("thank you for your datas");

            return RedirectToAction("Index");
        }

        public ActionResult Error(Dictionary<string, string> ErrorDetails)
        {
            var userData = new UserData();
            foreach (var kv in ErrorDetails ?? new Dictionary<string, string>())
                userData.Options.Add("ErrorDetails_" + kv.Key, kv.Value);
            Session.Store(userData);
            AddMessage("Error");

            return RedirectToAction("Index");
        }

        public ActionResult Logout(Dictionary<string, string> ErrorDetails)
        {
            FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);
            //var sam = new SessionAuthenticationModule();
            //sam.SignOut();
            return RedirectToAction("Index");
        }
    }
}
