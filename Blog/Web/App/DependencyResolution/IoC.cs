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
                //x.Scan(scan =>
                //{
                //    scan.TheCallingAssembly();
                //    scan.WithDefaultConventions();
                //});

                x.For<System.Web.Mvc.IControllerFactory>().Use<CustomControllerFactory>();

                //x.For<global::Raven.Client.IDocumentSession>().HybridHttpOrThreadLocalScoped()
                //    .Use(() => Dal.RavenDocumentStore.OpenSession());

                //x.FillAllPropertiesOfType<global::Raven.Client.IDocumentSession>();
                x.FillAllPropertiesOfType<Blog.Dal.BlogConfigRepo>();
                x.FillAllPropertiesOfType<Blog.Dal.UserRepo>();
            });

            ObjectFactory.AssertConfigurationIsValid();
            return ObjectFactory.Container;
        }
    }
}