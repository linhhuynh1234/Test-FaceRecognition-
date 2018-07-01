using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Lessons
    {
        public int code { get; set; }
        public List<Lesson> data { get; set; }
        public string message { get; set; }
        public class Lesson
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string info { get; set; }
        }
    }
}