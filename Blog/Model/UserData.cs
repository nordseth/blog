﻿using System;
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
        }

        public string Id { get; set; }
        public Dictionary<string,string> Options { get; set; }
    }
}