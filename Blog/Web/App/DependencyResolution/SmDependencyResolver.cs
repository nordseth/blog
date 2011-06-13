using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using Blog.Core;

namespace Blog.Web.App.DependencyResolution
{
    public class SmDependencyResolver : IDependencyResolver
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IContainer _container;

        public SmDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;
            try
            {
                var tmp = serviceType.IsAbstract || serviceType.IsInterface
                         ? _container.TryGetInstance(serviceType)
                         : _container.GetInstance(serviceType);
                Log.DebugFormat("Resolving {0} to {1}", serviceType, tmp ?? "(null)");
                return tmp;
            }
            catch
            {
                Log.DebugFormat("Error resovling {0}, returning null", serviceType);
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var tmp = _container.GetAllInstances(serviceType).Cast<object>();
            Log.DebugFormat("Resolving {0} to {1}", serviceType, tmp.ToStringList());
            return tmp;
        }

    }
}