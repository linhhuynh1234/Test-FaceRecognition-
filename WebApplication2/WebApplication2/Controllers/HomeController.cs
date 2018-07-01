using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        sep21t22Entities2 db = new sep21t22Entities2();
        public API api = new API();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {

            return View();
        }


    }
}