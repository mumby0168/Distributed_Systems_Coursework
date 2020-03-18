using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DistSysACWClient.Helpers;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    public class UserHandler
    {
        private readonly string _address;
        private readonly HttpClient _client;
        private readonly Regex _guidRegex;
        private readonly Regex _usernameRegex;

        public UserHandler(string address)
        {
            _address = address;
            _client = new HttpClient();
            _guidRegex = new Regex(@"([a-z0-9]+-){4}[a-z0-9]+");
            _usernameRegex = new Regex(@"^[A-Za-z0-9]+");
        }

        public async Task UserGet(string input)
        {
            string username = input.Replace("User Get", "").Trim();
            Console.WriteLine("... please wait");
            var res = await _client.GetResultAsStringAsync($"{_address}/user/new?username={username}");
            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Forbidden:
                    Console.WriteLine(res.Data);
                    break;
            }

        }

        public async Task UserPost(string input)
        {
            string username = input.Replace("User Post", "").Trim();
            Console.WriteLine("... please wait");
            var res = await _client.PostAsync($"{_address}/user/new",
                new StringContent(JsonConvert.SerializeObject(username), Encoding.Default, "application/json"));
            if (res.StatusCode == HttpStatusCode.OK)
            {
                
                var key = await res.Content.ReadAsStringAsync();
                User.Set(username, key);
                Console.WriteLine("Got API Key");
            }
            else if (res.StatusCode == HttpStatusCode.Forbidden)
            {
                var str = await res.Content.ReadAsStringAsync();
                Console.WriteLine(str);
            }
        }

        public Task UserSet(string input)
        {
            input = input.Replace("User Set ", "");
            var keyMatch = _guidRegex.Match(input);
            string key = string.Empty;
            if (keyMatch.Success)
            {
                key = keyMatch.Value;
            }

            string username = string.Empty;
            var usernameMatch = _usernameRegex.Match(input);
            if (usernameMatch.Success)
            {
                username = usernameMatch.Value;
            }

            if (username != string.Empty && key != string.Empty)
            {
                User.Set(username, key);
                Console.WriteLine("Stored");
            }
            else
            {
                Console.WriteLine("Please enter a valid username and key e.g. User Set <username> <valid-guid>");
            }
            
            return Task.CompletedTask;
        }

        public async Task DeleteUser()
        {
            if (!User.IsSet())
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;    
            }

            var client = User.CreateClient();
            Console.WriteLine("... please wait");
            var res = await client.DeleteAsync($"{_address}/user/removeuser?username={User.Username}");
            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                Console.WriteLine(result);
                User.Clear();
            }
            else if(res.StatusCode == HttpStatusCode.Unauthorized)
                Console.WriteLine("False");

        }

        public async Task ChangeUserRole(string input)
        {
            input = input.Replace("User Role ", "");

            if (!User.IsSet())
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }
            
            var matches = Regex.Matches(input, @"[a-zA-Z]+");
            if (matches.Count != 2)
            {
                Console.WriteLine("Please enter both a username and a role to change to.");
                return;
            }

            var client = User.CreateClient();
            Console.WriteLine("... please wait");
            var res = await client.PostAsync($"{_address}/user/changerole", new StringContent(
                JsonConvert.SerializeObject(new {username = matches[0].Value, Role = matches[1].Value }), Encoding.Default, "application/json"));

            if (res.IsSuccessStatusCode)
            {
                var str = await res.Content.ReadAsStringAsync();
                Console.WriteLine(str);
            }
            else if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("User Set does not have permissions to make perform this action.");
            }
            else if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                Console.WriteLine(await res.Content.ReadAsStringAsync());
            }
        }
        
    }
}