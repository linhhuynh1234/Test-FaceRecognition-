using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Logins
    {
        public int code { get; set; }
        public Login data { get; set; }
        public string message { get; set; }
        public class Login
        {
            public string Id { get; set; }
            public string secret { get; set; }

        }
    }
}