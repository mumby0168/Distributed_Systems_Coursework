using System.Threading.Tasks;
using DistSysACW.Models;

namespace DistSysACW.Repositorys
{
    public interface IUserRepository
    {
        Task<string> CreateUserAsync(string username);

        Task<bool> DoesUserExistAsync(string apiKey);

        Task<bool> DoesUserExistAsync(string apiKey, string username);

        Task<User> GetUserAsync(string apiKey);

        Task RemoveUserAsync(string apiKey);
    }
}