using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository, ILogger<AuthMiddleware> logger)
        {
            #region Task5
            
            //TODO: refactor this.

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
                
                if (stringKey == "Admin")
                {
                    logger.LogInformation($"User given admin role with username {user.Username}");
                    var claim = new Claim(ClaimTypes.Role, "Admin");
                    var name = new Claim(ClaimTypes.Name, user.Username);
                    context.User.AddIdentity(new ClaimsIdentity(new []{claim, name}));
                }
                else if (stringKey == "Standard")
                {
                    logger.LogInformation($"User given standard role with username {user.Username}");
                    var claim = new Claim(ClaimTypes.Role, "Standard");
                    var name = new Claim(ClaimTypes.Name, user.Username);
                    context.User.AddIdentity(new ClaimsIdentity(new []{claim, name}));
                }
            }
            
            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
