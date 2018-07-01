using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Models
{
  
    public class Item
    {
       
        public List<cThamdu> AvailableColours { get; set; }
    }
    public class cThamdu
    {
       public string Id { get; set; }
        public bool checkbox { get; set; }

    }
}