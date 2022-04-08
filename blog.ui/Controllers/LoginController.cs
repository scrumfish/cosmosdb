using blog.objects;
using blog.objects.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace blog.ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserData userData;

        public LoginController(IUserData data)
        {
            userData = data;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Credentials credentials)
        {
            if (credentials == null || 
                string.IsNullOrWhiteSpace(credentials.email) || 
                string.IsNullOrWhiteSpace(credentials.password))
            {
                return BadRequest();
            }
            var user = userData.GetUser(credentials.email);
            if (user == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrWhiteSpace(user.email))
            {
                return BadRequest();
            }
            if (BCrypt.Net.BCrypt.Verify(credentials.password, user.password))
            {
                user.password = null;
                var claimsIdentity = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, user.email),
                        new Claim(ClaimTypes.NameIdentifier, user.email)
                    }, "Password");
                if (user.roles != null)
                {
                    foreach (var role in user.roles)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).Wait();
                return Ok(user);
            }
            return BadRequest();
        }
    }
}
