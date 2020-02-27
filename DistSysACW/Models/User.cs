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
            
        }
        
        [Key]
        public string ApiKey { get; set; }
        
        public string Username { get; set; }
        
        public UserRole Role { get; set; }
        
        #endregion
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion


}