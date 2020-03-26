using System.Threading.Tasks;
using DistSysACW.Models;

namespace DistSysACW.Repositorys
{
    public class LogRepository : ILogRepository
    {
        private readonly UserContext _context;

        public LogRepository(UserContext context)
        {
            _context = context;
        }
        
        public Task AddAsync(Log log)
        {
            _context.Logs.Add(log);
            return _context.SaveChangesAsync();
        } 
    }
}