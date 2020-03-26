using System.Threading.Tasks;
using DistSysACW.Models;

namespace DistSysACW.Repositorys
{
    public interface ILogRepository
    {
        Task AddAsync(Log log);
    }
}