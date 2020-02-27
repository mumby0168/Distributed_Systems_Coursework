using DistSysACW.Names;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/test/")]
    public class TestController : ControllerBase
    {
        [HttpGet("admin-only")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AdminOnly()
        {
            return Ok();
        }
        
        [HttpGet("both")]
        [Authorize(Roles = Roles.All)]
        public IActionResult Both()
        {
            return Ok();
        }
    }
}