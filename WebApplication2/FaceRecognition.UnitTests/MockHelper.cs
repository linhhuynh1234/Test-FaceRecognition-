using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Web;
using System.Web.Mvc;

namespace FaceRecognition.UnitTests
{
    class MockHelper
    {
        public Mock<HttpSessionStateBase> Session { get; set; }
        private Mock<HttpContextBase> Context { get; set; }
        public Mock<ControllerContext> controllerContext { get; set; }
        public MockHelper()
        {
            Session = new Mock<HttpSessionStateBase>();
            Context = new Mock<HttpContextBase>();
            controllerContext = new Mock<ControllerContext>();
        }
        public Mock<HttpContextBase> MakeFakeContext()
        {
            Context.Setup(c => c.Session).Returns(Session.Object);
            return Context;
        }
        public Mock<ControllerContext> MakeFakeController()
        {
            controllerContext.Setup(c => c.HttpContext.Session).Returns(Session.Object);
            return controllerContext;
        }
    }
}
