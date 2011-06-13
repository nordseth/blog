using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Blog.Web.App.Start.Log4Net), "Start")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace Blog.Web.App.Start
{
    /// <summary>
    /// use this to log, or the log extention
    /// private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    /// </summary>
    public static class Log4Net
    {
        public static void Start()
        {
            Blog.Core.LogManager.SetFactoryMethod(
                t => new Log4NetAdapter(log4net.LogManager.GetLogger(t)));
        }
    }
}

