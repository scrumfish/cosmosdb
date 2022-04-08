using blog.objects;
using blog.objects.Interfaces;
using blog.ui.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace blog.ui.tests
{
    [TestClass]
    public class LoginControllerTests
    {
        private LoginController target = new (new Mock<IUserData>().Object);
        private Mock<IUserData> userMock = new();
        private Mock<ControllerContext>? controllerContextMock;
        private Mock<HttpContext>? httpContextMock;
        private Mock<HttpRequest>? httpRequestMock;
        private Mock<IServiceProvider>? serviceProviderMock;
        private Mock<IAuthenticationService>? authenticationServiceMock;
        private readonly string password = "testpassword";
        private readonly string hash = "$2a$12$P5nxl/VfzOxl04PrSb4JXO5Ygf/CKzBRS4c.JOQb3FOfEt1HLY.16";

        [TestInitialize]
        public void Setup()
        {
            serviceProviderMock = new Mock<IServiceProvider>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);
            httpRequestMock = new Mock<HttpRequest>();
            controllerContextMock = new Mock<ControllerContext>();
            httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(h => h.Request).Returns(httpRequestMock.Object);
            controllerContextMock.Object.HttpContext = httpContextMock.Object;
            httpRequestMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(h => h.RequestServices).Returns(serviceProviderMock.Object);
            userMock = new Mock<IUserData>();
            target = new LoginController(userMock.Object);
            target.ControllerContext = controllerContextMock.Object;
        }

        [TestMethod]
        public void PostReturnsBadRequestOnEmptyEmail()
        {
            var credentials = new Credentials
            {
                 password = "foo"
            };
            var result = target.Post(credentials) as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostReturnsBadRequestOnEmptyPassword()
        {
            var credentials = new Credentials
            {
                email = "foo"
            };
            var result = target.Post(credentials) as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostRetrievesUserFromDatabase()
        {
            var credentials = new Credentials
            {
                email = "foo",
                password = "bar"
            };
            target.Post(credentials);
            userMock.Verify(um => um.GetUser(It.Is<string>(e => e == "foo")));
        }

        [TestMethod]
        public void PostReturnsBadRequestOnNotFoundUser()
        {
            userMock.Setup(um => um.GetUser(It.IsAny<string>())).Returns<User>(null);
            var credentials = new Credentials
            {
                email = "foo",
                password = "bar"
            };
            var result = target.Post(credentials) as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfPasswordsDoNotMatch()
        {
            userMock.Setup(um => um.GetUser(It.IsAny<string>())).Returns(new User { password = hash});
            var credentials = new Credentials
            {
                email = "foo",
                password = "bar"
            };
            var result = target.Post(credentials) as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostReturnsOkObjectIfPasswordsMatch()
        {
            userMock.Setup(um => um.GetUser(It.IsAny<string>())).Returns(new User
            {
                password = hash,
                email = "test@example.com",
                roles = new[] { "admin" }
            });
            var credentials = new Credentials
            {
                email = "foo",
                password = password
            };
            var result = target.Post(credentials) as OkObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostReturnsUserInOkObject()
        {
            userMock.Setup(um => um.GetUser(It.IsAny<string>())).Returns(new User
            {
                password = hash,
                email = "test@example.com",
                roles = new[] { "admin" }
            });
            var credentials = new Credentials
            {
                email = "foo",
                password = password
            };
            var result = target.Post(credentials) as OkObjectResult;
            Assert.IsNotNull(result?.Value as User);
        }

        [TestMethod]
        public void PostNullsHashFromReturnObject()
        {
            userMock.Setup(um => um.GetUser(It.IsAny<string>())).Returns(new User
            {
                password = hash,
                email = "test@example.com",
                roles = new[] { "admin" }
            });
            var credentials = new Credentials
            {
                email = "foo",
                password = password
            };
            var result = target.Post(credentials) as OkObjectResult;
            Assert.IsNull((result?.Value as User)?.password);
        }
    }
}