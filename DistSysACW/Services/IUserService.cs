using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface IUserService
    {
        Task<string> CheckUserExists(string username);

        Task<string> CreateUser(string username);
    }
}