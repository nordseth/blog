using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Web.App.Raven
{
    public interface IRavenSessionFactoryBuilder
    {
        IRavenSessionFactory GetSessionFactory();
    }
}