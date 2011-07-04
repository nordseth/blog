using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Model
{
    public class SimpleClaim
    {
        public string ClaimType { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
    }
}
