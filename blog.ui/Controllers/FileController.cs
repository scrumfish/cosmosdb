using blog.objects.Interfaces;
using blog.ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blog.ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IImageData imageData { get; set; }

        public FileController(IImageData data)
        {
            imageData = data;
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Post([FromForm] FileModel file)
        {
            if (file == null || file.FormFile == null || string.IsNullOrWhiteSpace(file.Filename))
            {
                return BadRequest();
            }
            using (var stream = new MemoryStream())
            {
                file.FormFile.CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var uri = imageData.Save(file.Filename, stream);
                return Ok(new UploadResponse { data = new UploadLink { link = uri } });
            }
        }
    }
}
