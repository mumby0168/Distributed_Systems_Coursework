using System;
using System.Net;
using System.Threading.Tasks;
using DistSysACW.Exceptions;
using DistSysACW.Models;
using DistSysACW.Names;
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

        public async Task<bool> RemoveUser(string username, string apiKey)
        {
            var result = await _repository.GetUserAsync(apiKey);
            if (result.Username == username)
            {
                await _repository.RemoveUserAsync(apiKey);
                return true;
            }

            return false;
        }

        public async Task UpdateRole(string username, string role)
        {
            //TODO: check statement in spec stating all other error cases should return what is in the catch. What could be the other error cases?
            try
            {
                var user = await _repository.GetUserByUsernameAsync(username);
                if (user is null)
                {
                    throw new HttpStatusCodeException("NOT DONE: Username does not exist", HttpStatusCode.BadRequest);
                }

                if (role != Roles.User && role != Roles.Admin)
                {
                    throw new HttpStatusCodeException("NOT DONE: Role does not exist", HttpStatusCode.BadRequest);
                }

                user.Role = (UserRole)Enum.Parse(typeof(UserRole), role);
                await _repository.UpdateAsync(user);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(HttpStatusCodeException))
                    throw e;
                else
                {
                    throw new HttpStatusCodeException("NOT DONE: An error occured", HttpStatusCode.BadRequest);
                }
            }
            
        }
    }
}