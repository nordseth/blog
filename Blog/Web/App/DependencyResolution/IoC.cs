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
                            //        {
                            //            scan.TheCallingAssembly();
                            //            scan.WithDefaultConventions();
                            //        });

                            x.For<System.Web.Mvc.IControllerFactory>().Use<CustomControllerFactory>();

                            x.For<Blog.Web.App.Raven.IRavenSessionFactoryBuilder>().Singleton().Use<Blog.Web.App.Raven.RavenSessionFactoryBuilder>();

                            x.For<global::Raven.Client.IDocumentSession>().HybridHttpOrThreadLocalScoped()
                                .AddInstances(inst => inst.ConstructedBy(
                                    context => context.GetInstance<Blog.Web.App.Raven.IRavenSessionFactoryBuilder>().GetSessionFactory().CreateSession()));

                            x.FillAllPropertiesOfType<global::Raven.Client.IDocumentSession>();
                        });
            ObjectFactory.AssertConfigurationIsValid();
            return ObjectFactory.Container;
        }
    }
}