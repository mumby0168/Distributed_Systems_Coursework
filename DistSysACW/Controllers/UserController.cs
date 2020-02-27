using System.Threading.Tasks;
using DistSysACW.Models;
using DistSysACW.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/user/")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(UserContext context, IUserService userService) : base(context)
        {
            _userService = userService;
        }

        [HttpGet("new")]
        public async Task<IActionResult> CheckUserGet([FromQuery] string username)
        {
            return Ok(await _userService.CheckUserExists(username));
        }

        [HttpPost("new")]
        public async Task<IActionResult> CheckUserPost([FromBody] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Oops. Make sure your body contains a string " +
                                  "with your username and your Content-Type is Content-Type:application/json");
            }

            var key = await _userService.CreateUser(username);
            if (string.IsNullOrEmpty(key))
            {
                return Forbid("Oops. This username is already in use. Please try again with a new username.");
            }

            return Ok(key);
        }
    }
}