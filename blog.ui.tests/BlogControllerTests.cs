using blog.objects;
using blog.objects.Interfaces;
using blog.ui.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.ui.tests
{
    [TestClass]
    public class BlogControllerTests
    {
        private BlogController target = new(new Mock<IBlogData>().Object);
        private Mock<IBlogData> blogMock = new();

        [TestInitialize]
        public void Setup()
        {
            blogMock = new Mock<IBlogData>();
            target = new BlogController(blogMock.Object);
        }

        [TestMethod]
        public void PostFailsOnEmptyArticle()
        {
            var blog = new BlogEntry
            {
                title = "foo",
                article = string.Empty
            };
            var result = target.Post(blog) as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostFailsOnEmptyTitle()
        {
            var blog = new BlogEntry
            {
                title = string.Empty,
                article = "foo",
            };
            var result = target.Post(blog) as BadRequestResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostReturnsOkObjectResultForValidBlog()
        {
            var blog = new BlogEntry
            {
                title = "foo",
                article = "<h1>heh</h1>"
            };
            var result = target.Post(blog) as OkObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostPassesBlogToDAL()
        {
            var blog = new BlogEntry
            {
                title = "foo",
                article = "<h1>heh</h1>"
            };
            target.Post(blog);
            blogMock.Verify(b => b.Add(It.Is<BlogEntry>(be => be.Equals(blog))), Times.Once);
        }

        [TestMethod]
        public void PostReturnsNewId()
        {
            var expected = Guid.NewGuid().ToString();
            var blog = new BlogEntry
            {
                title = "foo",
                article = "<h1>heh</h1>"
            };
            blogMock.Setup(x => x.Add(It.IsAny<BlogEntry>())).Returns(expected);
            var result = ((target.Post(blog) as OkObjectResult)?.Value as IdResponse)?.id;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetReturnsOkObjectResultOnSuccess()
        {
            blogMock.Setup(b => b.GetTitles()).Returns(new List<Title> { new Title { title = "foo" } });
            var result = target.Get() as OkObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetReturnsNotFoundOnEmptyResult()
        {
            blogMock.Setup(b => b.GetTitles()).Returns(new List<Title>());
            var result = target.Get() as NotFoundResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetReturnsNotFoundOnNullResult()
        {
            blogMock.Setup(b => b.GetTitles()).Returns<List<Title>>(null);
            var result = target.Get() as NotFoundResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetIdReturnsOkObjectResultOnSuccess()
        {
            blogMock.Setup(b => b.Get(It.IsAny<string>())).Returns(new Blog());
            var result = target.Get("foo") as OkObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetIsReturnsNotFoundOnNullResult()
        {
            blogMock.Setup(b => b.Get(It.IsAny<string>())).Returns<Blog>(null);
            var result = target.Get("foo") as NotFoundResult;
            Assert.IsNotNull(result);
        }
    }
}