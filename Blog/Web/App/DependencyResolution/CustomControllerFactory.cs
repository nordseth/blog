using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using System.Web.Routing;
using System.Web.SessionState;
using Blog.Core;

namespace Blog.Web.App.DependencyResolution
{
    public class CustomControllerFactory : IControllerFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IControllerFactory _innerFactory;

        public CustomControllerFactory()
        {
            _innerFactory = new DefaultControllerFactory();
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                return _innerFactory.CreateController(requestContext, controllerName);
            }
            catch (Exception ex)
            {
                Log.Debug("CreateController failed", ex);
                IController errorController = DependencyResolver.Current.GetService<Blog.Web.Controllers.ErrorController>();
                ((Blog.Web.Controllers.ErrorController)errorController).InvokeHttp404(requestContext.HttpContext);
                return errorController;
            }
        }

        public void ReleaseController(IController controller)
        {
            _innerFactory.ReleaseController(controller);
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return _innerFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }
    }
}