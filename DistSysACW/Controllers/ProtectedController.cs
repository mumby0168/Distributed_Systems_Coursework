using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DistSysACW.Names;
using DistSysACW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/protected/")]
    public class ProtectedController : ControllerBase
    {
        private readonly IRsaProvider _rsaProvider;

        public ProtectedController(IRsaProvider rsaProvider)
        {
            _rsaProvider = rsaProvider;
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
            var bytes = Encoding.ASCII.GetBytes(message);
            var encoded = provider.ComputeHash(bytes, 0, bytes.Length);

            var str = BitConverter.ToString(encoded);
            str = str.Replace("-", "");
            return Ok(str);

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
            var bytes = Encoding.ASCII.GetBytes(message);
            var encoded = provider.ComputeHash(bytes, 0, bytes.Length);
            
            var str = BitConverter.ToString(encoded);
            str = str.Replace("-", "");
            return Ok(str);

        }


        [HttpGet("getpublickey")]
        [Authorize(Roles = Roles.All)]
        public IActionResult PublicKey() => Ok(_rsaProvider.PublicKey);

        [HttpGet("sign")]
        [Authorize(Roles = Roles.All)]
        public IActionResult Sign([FromQuery]string message)
        {
            return Ok(_rsaProvider.Sha1Sign(message));
        }

    }
}