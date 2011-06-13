//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;
//using Blog.Core;
//using StructureMap;

//namespace Blog.Web.App.DependencyResolution
//{
//    // http://stackoverflow.com/questions/619895/how-can-i-properly-handle-404s-in-asp-net-mvc/2577095#2577095
//    public class CustomControllerActivator : IControllerActivator
//    {
//        public class StructureMapControllerActivator : IControllerActivator
//        {
//            public StructureMapControllerActivator(IContainer container)
//            {
//                _container = container;
//            }

//            private IContainer _container;

//            public IController Create(RequestContext requestContext, Type controllerType)
//            {
//                return _container.GetInstance(controllerType) as IController;
//            }
//        }

//        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

//        IController IControllerActivator.Create(RequestContext requestContext, Type controllerType)
//        {
//            try
//            {
//                if (controllerType == null)
//                {
//                    return DependencyResolver.Current.GetService(controllerType) as IController;
//                }
//            }
//            catch (HttpException ex)
//            {
//                if (ex.GetHttpCode() == 404)
//                {
//                    IController errorController = DependencyResolver.Current.GetService<Blog.Web.Controllers.ErrorController>();
//                    ((Blog.Web.Controllers.ErrorController)errorController).InvokeHttp404(requestContext.HttpContext);

//                    return errorController;
//                }
//                else
//                    throw ex;
//            }

//            return DependencyResolver.Current.GetService(controllerType) as IController;
//        }

//    }
//}