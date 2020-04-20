using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DistSysACW.Names;
using DistSysACW.Repositorys;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal.Account;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Org.BouncyCastle.Asn1.X509;

namespace DistSysACW.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository, ILogger<AuthMiddleware> logger, ILogRepository logRepository)
        {
            #region Task5

            var key = context.Request.Headers["ApiKey"];
            if (key.Any())
            {
                var stringKey = key.ToString();
                var user = await userRepository.GetUserAsync(stringKey);
                if (user is null)
                {
                    logger.LogInformation($"No user found with api key {stringKey}");
                    await _next(context);
                    return;
                }
                
                if (user.Role.ToString() == Roles.Admin)
                {
                    logger.LogInformation($"User given admin role with username {user.Username}");
                    var claim = new Claim(ClaimTypes.Role, Roles.Admin);
                    var name = new Claim(ClaimTypes.Name, user.Username);
                    var keyClaim = new Claim(ClaimTypes.NameIdentifier, stringKey);
                    context.User.AddIdentity(new ClaimsIdentity(new []{claim, name, keyClaim}));
                    
                    var log = user.CreateLog($"User Requested: {context.Request.Path.Value}");
                    await logRepository.AddAsync(log);
                }
                else if (user.Role.ToString() == Roles.User)
                {
                    logger.LogInformation($"User given standard role with username {user.Username}");
                    var claim = new Claim(ClaimTypes.Role, Roles.User);
                    var name = new Claim(ClaimTypes.Name, user.Username);
                    var keyClaim = new Claim(ClaimTypes.NameIdentifier, stringKey);
                    context.User.AddIdentity(new ClaimsIdentity(new []{claim, name, keyClaim}));
                    var log = user.CreateLog($"User Requested: {context.Request.Path.Value}");
                    await logRepository.AddAsync(log);
                }
            }
            
            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
