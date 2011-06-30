using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;

namespace Blog.Web.App.Raven
{
    public interface IRavenSessionFactory
    {
        IDocumentSession CreateSession();
    }
}