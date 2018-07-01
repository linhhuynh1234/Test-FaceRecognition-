using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DiemDanhview
    {
        public string MSSV { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public bool? DiemDanh { get; set; }
    }
}