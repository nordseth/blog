using System.Web.Mvc;
using StructureMap;
using Blog.Web.App.DependencyResolution;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Blog.Web.App.Start.StructuremapMvc), "Start")]

namespace Blog.Web.App.Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = (IContainer)IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}