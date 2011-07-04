using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Document;

namespace Blog.Dal
{
    public static class RavenDocumentStore
    {
        private static IDocumentStore _store;

        public static IDocumentSession OpenSession()
        {
            return (_store ?? (_store = CreateDocumentStore())).OpenSession();
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

            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(RavenDocumentStore).Assembly, store);

            return store;
        }

    }
}
