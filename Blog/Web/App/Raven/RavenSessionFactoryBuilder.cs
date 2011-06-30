using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Document;
using Raven.Client;
using Raven.Client.Embedded;

namespace Blog.Web.App.Raven
{
    public class RavenSessionFactoryBuilder : IRavenSessionFactoryBuilder
    {
        private IRavenSessionFactory _ravenSessionFactory;

        public IRavenSessionFactory GetSessionFactory()
        {
            return _ravenSessionFactory ?? (_ravenSessionFactory = CreateSessionFactory());
        }

        private static IRavenSessionFactory CreateSessionFactory()
        {
            return new RavenSessionFactory(CreateDocumentStore());
        }

        private static IDocumentStore CreateDocumentStore()
        {
            IDocumentStore store;

            if (AppConfigWrapper.UseEmbeddedDatabase)
            {
                store = new EmbeddableDocumentStore { DataDirectory = AppConfigWrapper.DataPath }.Initialize();
            }
            else
            {
                store = new DocumentStore { ConnectionStringName = AppConfigWrapper.RavenDBConnectionString.Name }.Initialize();
            }

            //IndexCreation.CreateIndexes(typeof(Tags_Count).Assembly, store);

            return store;
        }

    }
}