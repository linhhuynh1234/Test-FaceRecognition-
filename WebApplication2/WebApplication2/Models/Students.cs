using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Students
    {
        public int code { get; set; }
        public List<Student> data { get; set; }
        public string message { get; set; }
        public class Student
        {
            public string id { get; set; }
            public string fullname { get; set; }
            public string birthday { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
        }
    }
}