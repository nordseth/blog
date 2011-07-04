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
        public BlogConfigRepo()
        {
        }

        public BlogConfigRepo(IDocumentSession session)
        {
            Session = session;
        }

        public IDocumentSession Session { get; set; }

        public BlogConfig GetConfig()
        {
            var blogConfig = Session.Load<BlogConfig>("Blog/Config");
            if (blogConfig == null)
            {
                blogConfig = new BlogConfig();
                blogConfig.Id = "Blog/Config";
                Session.Store(blogConfig);
            }
            return blogConfig;
        }

        public void UpdateConfig(BlogConfig blogConfig)
        {
            blogConfig.Id = "Blog/Config";
            Session.Store(blogConfig);
        }
    }
}
