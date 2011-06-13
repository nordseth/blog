using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Web.ViewModels
{
    public class MenuViewModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }
    }
}