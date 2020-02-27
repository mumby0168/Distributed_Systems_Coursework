using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface IUserService
    {
        Task<string> CheckUserExists(string username);

        Task<string> CreateUser(string username);

        Task<bool> RemoveUser(string username, string apiKey);

        Task UpdateRole(string username, string role);
    }
}