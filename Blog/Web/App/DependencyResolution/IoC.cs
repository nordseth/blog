using StructureMap;
using Blog.Core;

namespace Blog.Web.App.DependencyResolution
{
    public static class IoC
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<System.Web.Mvc.IControllerFactory>().Use<CustomControllerFactory>();
                        });
            ObjectFactory.AssertConfigurationIsValid();
            return ObjectFactory.Container;
        }
    }
}