using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DistSysACW.Dto;
using DistSysACW.Exceptions;
using DistSysACW.Models;
using DistSysACW.Names;
using DistSysACW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [AllowAnonymous]
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
                throw new HttpStatusCodeException("Oops. This username is already in use. Please try again with a new username.", HttpStatusCode.Forbidden);
            }

            return Ok(key);
        }

        [Authorize(Roles = Roles.All)]
        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser([FromQuery] string username)
        {
            string key = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _userService.RemoveUser(username, key));
        }

        [HttpPost("ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto dto)
        {
            await _userService.UpdateRole(dto.Username, dto.Role);
            return Ok("DONE");
        }
        
    }
}