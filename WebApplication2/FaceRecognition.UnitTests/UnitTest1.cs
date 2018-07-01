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
        public async Task ValidateLogin()
        {
            //login website
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();

            //var test = new Mock<TestLesson>();

            var controller = new AccountController();
            var Username = "phamminhhuyen";
            var password = "croprokiwi";
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            //var redirectRoute = controller.Login(Username, password) as Task<ActionResult>;

            var redi = await controller.Login(Username, password) as RedirectToRouteResult;
            //
            Assert.IsNotNull(redi);
            Assert.AreEqual("Open", redi.RouteValues["action"]);
            Assert.AreEqual("MonHoc", redi.RouteValues["controller"]);
        }

        [TestMethod]
        public void ValidateViewCourseTeaching()
        {
            // xem danh sach mon hoc dang day
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
        public void ValidateViewLesson()
        {
            // xem mon hoc
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.MonHocController();
            //string id = "SP";

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
        public void ValidateViewListAttendance()
        {
            //xem danh sach diem danh
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();

            var lession = db.BuoiHocs.FirstOrDefault(x => x.MaKH == "MH1").ID_BH;

            //var lesstion = db.BuoiHocs.LastOrDefault(x => x.MaKH == "MH1").ID_BH;

            string id = lession.ToString();

            var result = controller.ListDiemDanh(id) as ViewResult;
            Assert.AreEqual("", result.ViewName.ToString());
        }

        [TestMethod]
        public void ValidateViewListStudent()
        {
            //xem danh sach sinh vien
            var helper = new MockHelper();
            var context = helper.MakeFakeContext();
            var controller = new WebApplication2.Controllers.CourseController();
            //string id = "SP";

            context.SetupGet(x => x.Session["MaKH"]).Returns("MH2");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            ViewResult result = controller.ListStudent("MH2") as ViewResult;
            
            //var controller = new WebApplication2.Controllers.CourseController();
            // var Student = db.ThamDus.FirstOrDefault(x => x.MaKH == "MH2");
            // string id = Student.ToString();
            // var result = controller.ListStudent(id) as ViewResult;
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
        public void ValidateChangeAttendance()
        {
            //thay doi trang thai diem danh
            var controller = new WebApplication2.Controllers.CourseController();

            var ID_DD = db.DiemDanhs.FirstOrDefault(x => x.MSSV == "T153337").ID;

            string id = ID_DD.ToString();

            var result = controller.Change(id) as RedirectToRouteResult;
            Assert.AreEqual("ListDiemDanh", result.RouteValues["action"]);

        }

        [TestMethod]
        public void ValidateChangeAttendanceSession()
        {
            // thay doi buoi diem danh
            var controller = new WebApplication2.Controllers.CourseController();
            var Buoithu = db.BuoiHocs.FirstOrDefault(x => x.MaKH == "MH2").ID_BH;


            string id = Buoithu.ToString();

            var result = controller.edit(id) as RedirectToRouteResult;
            Assert.AreEqual("ListDiemDanh", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task LoginInvalidAccount()
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

    }
}

