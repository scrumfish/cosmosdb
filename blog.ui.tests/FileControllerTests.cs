using blog.objects.Interfaces;
using blog.ui.Controllers;
using blog.ui.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.ui.tests
{
    [TestClass]
    public class FileControllerTests
    {
        private FileController target = new(new Mock<IImageData>().Object);
        private Mock<IImageData> imageData = new();

        [TestInitialize]
        public void Setup()
        {
            imageData = new Mock<IImageData>();
            target = new FileController(imageData.Object);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfFilenameNull()
        {
            var file = new FileModel
            {
                Filename = null,
                FormFile = new Mock<IFormFile>().Object
            };
            var result = target.Post(file) as BadRequestResult;
        }

        [TestMethod]
        public void PostReturnsBadRequestIfFilenameEmpty()
        {
            var file = new FileModel
            {
                Filename = string.Empty,
                FormFile = new Mock<IFormFile>().Object
            };
            var result = target.Post(file) as BadRequestResult;
        }

        [TestMethod]
        public void PostReturnsBadRequestIfFormFileNull()
        {
            var file = new FileModel
            {
                Filename = "foo.png",
                FormFile = null
            };
            var result = target.Post(file) as BadRequestResult;
        }

        [TestMethod]
        public void PostPassesFileNameToDAL()
        {
            var expected = "foo.png";
            var file = new FileModel
            {
                Filename = expected,
                FormFile = new Mock<IFormFile>().Object
            };
            target.Post(file);
            imageData.Verify(i => i.Save(It.Is<string>(s => s == expected), It.IsAny<Stream>()), Times.Once);
        }

        [TestMethod]
        public void PostPassesStreamToDAL()
        {
            var file = new FileModel
            {
                Filename = "foo.png",
                FormFile = new Mock<IFormFile>().Object
            };
            target.Post(file);
            imageData.Verify(i => i.Save(It.IsAny<string>(), It.Is<Stream>(s => s != null)), Times.Once);
        }

        [TestMethod]
        public void PostCopiesToStream()
        {
            var formFile = new Mock<IFormFile>();
            var file = new FileModel
            {
                Filename = "foo.png",
                FormFile = formFile.Object
            };
            target.Post(file);
            formFile.Verify(f => f.CopyTo(It.IsAny<Stream>()), Times.Once);
        }

        [TestMethod]
        public void PostReturnsUploadResponse()
        {
            var file = new FileModel
            {
                Filename = "foo.png",
                FormFile = new Mock<IFormFile>().Object
            };
            var result = (target.Post(file) as OkObjectResult)?.Value as UploadResponse;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostReturnsUrlInResponse()
        {
            var expected = "foobar";
            imageData.Setup(i => i.Save(It.IsAny<string>(),It.IsAny<Stream>())).Returns(expected);
            var file = new FileModel
            {
                Filename = "foo.png",
                FormFile = new Mock<IFormFile>().Object
            };
            var result = (target.Post(file) as OkObjectResult)?.Value as UploadResponse;
            Assert.AreEqual(expected, result?.data.link);
        }
    }
}
