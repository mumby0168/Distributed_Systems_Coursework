using System;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace DistSysACW.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<string> CreateUserAsync(string username)
        {
            var key = Guid.NewGuid().ToString();

            var users = await _context.Users.ToListAsync();

            var user = new User()
            {
                Username = username,
                ApiKey = key
            };
            
            //TODO: in spec but not in step by step guide
            if (!users.Any())
            {
                user.Role = UserRole.Admin;
            }
            
            await _context.Users.AddAsync(user);
            
            return key;
        }

        public async Task<bool> DoesUserExistAsync(string apiKey)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
            return user != null;
        }

        public async Task<bool> DoesUserExistAsync(string apiKey, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey && u.Username == username);
            return user != null;
        }

        public async Task<bool> DoesUsernameExistAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null;
        }

        public Task<User> GetUserAsync(string apiKey)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
        }

        public async Task RemoveUserAsync(string apiKey)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);

            if (user is null)
            {    
                //TODO: consider what happens in this case may be in the spec.
                throw new Exception("There was no user with the given api key.");    
            }
            
            _context.Users.Remove(user);
        }
    }
}