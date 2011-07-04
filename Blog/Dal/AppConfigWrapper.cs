using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Blog.Dal
{
    internal static class AppConfigWrapper
    {
        public static string DataPath { get { return ConfigurationManager.AppSettings["DataPath"]; } }
        public static bool UseEmbeddedDatabase { get { return (ConfigurationManager.AppSettings["UseEmbeddedDatabase"] ?? "").ToLower() == "true"; } }
        public static ConnectionStringSettings RavenDBConnectionString { get { return ConfigurationManager.ConnectionStrings["RavenDB"]; } }
    }
}
