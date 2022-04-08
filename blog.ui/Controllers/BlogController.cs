using blog.objects;
using blog.objects.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace blog.ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private IBlogData blogData;

        public BlogController(IBlogData data)
        {
            blogData = data;
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Post([FromBody] BlogEntry entry)
        {
            if (entry == null ||
                string.IsNullOrWhiteSpace(entry.title) ||
                string.IsNullOrWhiteSpace(entry.article))
            {
                return BadRequest();
            }
            var result = blogData.Add(entry);
            return Ok(new IdResponse { id = result });
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = blogData.GetTitles();
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = blogData.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public void Delete(string id)
        {
            blogData.Delete(id);
        }
    }
}