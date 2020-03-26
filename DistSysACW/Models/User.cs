using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class User
    {
        #region Task2

        public User()
        {
            Logs = new List<Log>();
        }
        
        [Key]
        public string ApiKey { get; set; }
        
        public string Username { get; set; }
        
        public UserRole Role { get; set; }
        
        public ICollection<Log> Logs { get; set; }

        public Log CreateLog(string message)
        {
            return new Log
            {
                UserId = ApiKey,
                LogString = message,
                LogDateTime = DateTime.Now
            };
        }
        
        #endregion
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion


}
