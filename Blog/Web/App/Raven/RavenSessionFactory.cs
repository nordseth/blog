using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;

namespace Blog.Web.App.Raven
{
    public class RavenSessionFactory : IRavenSessionFactory
    {
        private readonly IDocumentStore _documentStore;

        public RavenSessionFactory(IDocumentStore documentStore)
        {
            if (_documentStore == null)
            {
                _documentStore = documentStore;
                _documentStore.Initialize();
            }
        }

        public IDocumentSession CreateSession()
        {
            return _documentStore.OpenSession();
        }
    }
}