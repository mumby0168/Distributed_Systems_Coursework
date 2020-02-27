using System.Threading.Tasks;
using DistSysACW.Repositorys;

namespace DistSysACW.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        private const string CheckUserTrueResponse =
            "True - User Does Exist! Did you mean to do a POST to create a new user?";

        private const string CheckUserFalseResponse =
            "False - User Does Not Exist! Did you mean to do a POST to create a new user?";

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        
        
        public async Task<string> CheckUserExists(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return CheckUserFalseResponse;
            }

            var result = await _repository.DoesUsernameExistAsync(username);

            return result ? CheckUserTrueResponse : CheckUserFalseResponse;
        }

        public async Task<string> CreateUser(string username)
        {
            var res = await _repository.DoesUsernameExistAsync(username);
            if (res) return "";
            return await _repository.CreateUserAsync(username);
        }
    }
}