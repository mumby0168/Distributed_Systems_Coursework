using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

        [Authorize(Roles = Roles.All)]
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
        
        [Authorize(Roles = Roles.All)]
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

        [HttpGet("addfifty")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AddFifty([FromQuery] string encryptedInteger, [FromQuery] string encryptedsymkey,
            [FromQuery] string encryptedIV)

        {
            var integerBytes = _rsaProvider.Decrypt(encryptedInteger);
            var iv = _rsaProvider.Decrypt(encryptedIV);
            var key = _rsaProvider.Decrypt(encryptedsymkey);

            var integer = BitConverter.ToInt16(integerBytes);
            integer += 50;
            string encryptedResult;
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
            aesProvider.Key = key;
            aesProvider.IV = iv;
            ICryptoTransform encryptor = aesProvider.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(integer);
                    }

                    encryptedResult = BitConverter.ToString(ms.ToArray());
                }
            }
            

            return Ok(encryptedResult);
        }

    }
}