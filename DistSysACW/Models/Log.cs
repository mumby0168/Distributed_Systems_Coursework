using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistSysACW.Models
{
    public class Log
    {
        [Key]
        public string LogId { get; set; }
        
        public string LogString { get; set; }
        
        public DateTime LogDateTime { get; set; }
        
        
        public User User { get; set; }
        
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
    }
}