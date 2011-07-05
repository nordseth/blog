using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Model
{
    public class UserData
    {
        public UserData()
        {
            Options = new Dictionary<string, string>();
            IdentityIds = new List<string>();
            Claims = new List<SimpleClaim>();
        }

        public string Id { get; set; }
        public bool Registerd { get; set; }
        public Dictionary<string, string> Options { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> IdentityIds { get; set; }
        public List<SimpleClaim> Claims { get; set; }
    }
}
