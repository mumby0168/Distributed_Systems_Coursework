using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/talkback/")]
    public class TalkBackController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context) : base(context) { }


        [HttpGet("Hello")]
        public string Get()
        {
            #region TASK1

            return "Hello World";

            #endregion
        }

        [HttpGet("Sort")]
        public IActionResult Get([FromQuery]string[] integers)
        {
            
            #region TASK1

            var numbers = new List<int>();

            foreach (var item in integers)
            {
                int num;
                if(int.TryParse(item, out num))
                {
                    numbers.Add(num);
                }
                else
                {
                    return BadRequest("Bad Request");
                }
            }

            numbers.Sort();


            return Ok(numbers);

            #endregion
        }
    }
}
