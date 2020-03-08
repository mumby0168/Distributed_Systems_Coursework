using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DistSysACW.Names;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/protected/")]
    public class ProtectedController : ControllerBase
    {
        public ProtectedController()
        {
            
        }

        [HttpGet("hello")]
        [Authorize(Roles = Roles.All)]
        public IActionResult Hello()
        {
            string username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            return Ok($"Hello {username}");
        }

        [HttpGet("sha1")]
        public IActionResult Sha1([FromQuery]string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Bad Request");
            }
            
            SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
            provider.Initialize();
            var bytes = Encoding.UTF8.GetBytes(message);
            var encoded = provider.ComputeHash(bytes, 0, bytes.Length);
            
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return Ok(builder.ToString());

        }
        
        [HttpGet("sha256")]
        public IActionResult Sha256([FromQuery]string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Bad Request");
            }
            
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
            provider.Initialize();
            var bytes = Encoding.UTF8.GetBytes(message);
            var encoded = provider.ComputeHash(bytes, 0, bytes.Length);
            
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return Ok(builder.ToString());

        }
        
    }
}