using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Blog.Model;

namespace Blog.Dal
{
    public class BlogConfigRepo
    {
        public BlogConfig GetConfig()
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                var blogConfig = session.Load<BlogConfig>("Blog/Config");
                if (blogConfig == null)
                {
                    blogConfig = new BlogConfig();
                    blogConfig.Id = "Blog/Config";
                    session.Store(blogConfig);
                    session.SaveChanges();
                }
                return blogConfig;
            }
        }

        public void UpdateConfig(BlogConfig blogConfig)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                blogConfig.Id = "Blog/Config";
                session.Store(blogConfig);
            }
        }
    }
}
