using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication2.Controllers;
using System.Threading.Tasks;
using WebApplication2.Models;
using System.Linq;

namespace FaceRecognition.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        sep21t22Entities2 db = new sep21t22Entities2();
        [TestMethod]
        public async Task Login()
        {
            //login website
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();

            var controller = new AccountController();
            var Username = "phamminhhuyen";
            var password = "croprokiwi";
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
           
            var redi = await controller.Login(Username, password) as RedirectToRouteResult;
            //
            Assert.IsNotNull(redi);
            Assert.AreEqual("Open", redi.RouteValues["action"]);
            Assert.AreEqual("MonHoc", redi.RouteValues["controller"]);
        }

        [TestMethod]
        public void Index()
        {
            // xem danh sach khoa hoc dang day
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();
            string id = "SP";

            context.SetupGet(x => x.Session["MaGV"]).Returns("MH");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            ViewResult result = controller.Index(id) as ViewResult;
            Assert.AreEqual("", result.ViewName.ToString());
        }

        [TestMethod]
        public void Open()
        {
            // xem mon hoc dang day
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.MonHocController();
  
            context.SetupGet(x => x.Session["ID"]).Returns("Software Project");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            ViewResult result = controller.Open() as ViewResult;
            Assert.AreEqual("", result.ViewName.ToString());
        }

        [TestMethod]
        public void ValidateViewListSession()
        {
            // xem list buoi
            var controller = new WebApplication2.Controllers.CourseController();
            string id = "MH1";

            var result = controller.ListBuoi(id) as ViewResult;

            Assert.AreEqual("", result.ViewName.ToString());
        }

        [TestMethod]
        public void ValidateViewListAttendance_Fail()
        {
            //xem danh sach diem danh
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();

            context.SetupGet(x => x.Session["MaKH"]).Returns("MH1");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            var result = controller.ListDiemDanh("107") as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Course", result.RouteValues["controller"]);

        }
         [TestMethod]
        public void ValidateViewListAttendance_success()
        {
            //xem danh sach diem danh
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();

            context.SetupGet(x => x.Session["MaKH"]).Returns("MH1");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
      
            var result = controller.ListDiemDanh("1071") as ViewResult;
            Assert.AreEqual("",result.ViewName);

        }

        [TestMethod]
        public void ValidateViewListStudent()
        {
            //xem danh sach sinh vien
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();

            context.SetupGet(x => x.Session["MaKH"]).Returns("MH2");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            ViewResult result = controller.ListStudent("MH2") as ViewResult;
           
            Assert.AreEqual("", result.ViewName);
        }


        [TestMethod]
        public void Logout()
        {
            //Logout website
            var helper = new MockHelper();
            var context = helper.MakeFakeController();
            var controller = new WebApplication2.Controllers.AccountController();
            controller.ControllerContext = context.Object;
            //
            var redirectToRouteResult = controller.LogOut() as RedirectToRouteResult;
            //Assert
            Assert.AreEqual("Login", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("Account", redirectToRouteResult.RouteValues["controller"]);
        }

        [TestMethod]
        public void Change()
        {
            //thay doi trang thai diem danh
            var controller = new WebApplication2.Controllers.CourseController();

            var ID_DD = db.DiemDanhs.FirstOrDefault(x => x.MSSV == "T153337").ID;

            string id = ID_DD.ToString();

            var result = controller.Change(id) as RedirectToRouteResult;
            Assert.AreEqual("ListDiemDanh", result.RouteValues["action"]);

        }

        [TestMethod]
        public void ValidateEdit()
        {
            // thay doi buoi diem danh
            var controller = new WebApplication2.Controllers.CourseController();
            var Buoithu = db.BuoiHocs.FirstOrDefault(x => x.MaKH == "MH2").ID_BH;

            string id = Buoithu.ToString();

            var result = controller.edit(id) as RedirectToRouteResult;
            Assert.AreEqual("ListDiemDanh", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task LoginInvalidAccount_Fail()
        {
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new AccountController();
            var Username = "phamminhhuyen";
            var password = " ";
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var redi = await controller.Login(Username, password) as RedirectToRouteResult;
            Assert.AreEqual("Login",redi.RouteValues["Action"]);
            Assert.AreEqual("Account", redi.RouteValues["controller"]);
        }

        [TestMethod]
        public void DiemDanh()
        {
            // hien thi danh sach de diem danh thu cong diem danh
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();
            
            var result = controller.DIEMDANH("MH1") as ViewResult;
            Assert.AreEqual("", result.ViewName.ToString());
        }
        [TestMethod]
        public async Task SynMember()
        {
            // dong bo sinh vien
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();
           
            context.SetupGet(x => x.Session["MaGV"]).Returns("MH");
            context.SetupGet(x => x.Session["secret"]).Returns("1655478314");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = await controller.SynMember("MH1") as RedirectToRouteResult;
            Assert.AreEqual("ListStudent", result.RouteValues["Action"]);
            Assert.AreEqual("Course", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void Ed()
        {
            // diem danh thu cong
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();
           
            context.SetupGet(x => x.Session["MaGV"]).Returns("MH");
            context.SetupGet(x => x.Session["MaKH"]).Returns("MH1");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.Ed("MH1") as RedirectToRouteResult;
            Assert.AreEqual("ListStudent", result.RouteValues["Action"]);
            
        }


        [TestMethod]
        public async Task SyncAttendanceAsync()
        {
            // dong bo diem danh
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();
            
            context.SetupGet(x => x.Session["MaGV"]).Returns("MH");
            context.SetupGet(x => x.Session["secret"]).Returns("1655478314");
            context.SetupGet(x => x.Session["MaKH"]).Returns("MH1");
            context.SetupGet(x => x.Session["BH"]).Returns("1071");

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = await controller.syncAttendanceAsync() as RedirectToRouteResult;
            Assert.AreEqual("ListDiemDanh", result.RouteValues["Action"]);
        }

    }
}

