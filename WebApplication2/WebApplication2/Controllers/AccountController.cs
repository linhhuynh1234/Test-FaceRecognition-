using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        sep21t22Entities2 db = new sep21t22Entities2();
        API api = new API();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Login(string username, string password)
        {
            var checkId = api.Check_Login(username, password);
            var secret = api.LoginSecret(username, password);
            if (checkId != null)
            {
                try {
                    var xz = await api.TestLesson(checkId.data.Id);
                    Session["MaGV"] = checkId.data.Id;
                    Session["secret"] = secret;

                    foreach (var item in xz)
                    {
                        Session["ID"] = item.Name;
                    }


                    Session["name"] = username;
                    return RedirectToAction("Open", "MonHoc");
                }
                catch (Exception e) {
                    //throw e;
                    TempData["Error"] = ("Username or password is wrong");
                    return RedirectToAction("Login", "Account");
                }
             
                //s  Session["monhoc"] = user.GiangDays.FirstOrDefault(x => x.ID_GiangVien == user.ID).ID_MonHoc;
              
            }

            //var user = db.GiangViens.FirstOrDefault(x => x.Username == username);
            //if (user != null)
            //{
            //    if (user.Password.Equals(password))
            //    {
            //        Session["ID"] = user.ID;
            //        Session["name"] = user.TenGV;
            //        Session["monhoc"] = user.GiangDays.FirstOrDefault(x => x.ID_GiangVien == user.ID).ID_MonHoc;
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session["ID"] = null;
            Session["name"] = null;
            Session["monhoc"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}
