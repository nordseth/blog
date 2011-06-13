using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Web.ViewModels
{
    public class BlogConfigViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string AnalyticsId { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }
}