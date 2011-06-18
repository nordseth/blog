using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Blog.Web
{
    public static class AppConfigWrapper
    {
        public static string DataPath { get { return ConfigurationManager.AppSettings["DataPath"]; } }
        public static bool UseEmbeddedDatabase { get { return ConfigurationManager.AppSettings["UseEmbeddedDatabase"].ToLower() == "true"; } }
        public static string HrdFeed { get { return string.Format(ConfigurationManager.AppSettings["HrdFeed"], AcsNamespace, AuthRealm); } }
        public static string AuthRealm { get { return ConfigurationManager.AppSettings["AuthRealm"]; } }
        public static string AcsNamespace { get { return ConfigurationManager.AppSettings["AcsNamespace"]; } }
        public static ConnectionStringSettings RavenDBConnectionString { get { return ConfigurationManager.ConnectionStrings["RavenDB"]; } }
    }
}