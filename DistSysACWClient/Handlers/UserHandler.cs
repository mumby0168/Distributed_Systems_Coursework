using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistSysACWClient.Helpers;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    public class UserHandler
    {
        private readonly string _address;
        private readonly HttpClient _client;
        
        public UserHandler(string address)
        {
            _address = address;
            _client = new HttpClient();
        }

        public async Task UserGet(string input)
        {
            string username = input.Replace("User Get", "").Trim();
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
            var res = await _client.PostAsync($"{_address}/user/new", new StringContent(JsonConvert.SerializeObject(username), Encoding.Default, "application/json"));
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
        
        
    }
}