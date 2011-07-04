using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Claims;

namespace Blog.Model
{
    public class UserData
    {
        public UserData()
        {
            Options = new Dictionary<string, string>();
            Ids = new Dictionary<string, List<SimpleClaim>>();
        }

        public string Id { get; set; }
        public bool Registerd { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Dictionary<string, List<SimpleClaim>> Ids { get; set; }
    }
}
