using System;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DistSysACWClient.Helpers;

namespace DistSysACWClient
{
    public class ProtectedHandler
    {
        private readonly string _address;
        private readonly HttpClient _client;

        public ProtectedHandler(string address)
        {
            _address = address;
            _client = new HttpClient();
        }

        public async Task Hello()
        {
            if (!User.IsSet())
            {
                Console.WriteLine(Constants.UserSetupFirst);
                return;
            }
            
            var client = User.CreateClient();
            var result = await client.GetResultAsStringAsync($"{_address}/protected/hello");
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(result.Data);
            }
        }

        public async Task Sha1(string input)
        {
            if (!User.IsSet())
            {
                Console.WriteLine(Constants.UserSetupFirst);
                return;
            }
            
            input = input.Replace("Protected SHA1 ", "");

            var client = User.CreateClient();
            var res = await client.GetResultAsStringAsync($"{_address}/protected/sha1?message={input}");
            if (res.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(res.Data);
            }
        }
        
        public async Task Sha256(string input)
        {
            if (!User.IsSet())
            {
                Console.WriteLine(Constants.UserSetupFirst);
                return;
            }
            
            input = input.Replace("Protected SHA256 ", "");

            var client = User.CreateClient();
            var res = await client.GetResultAsStringAsync($"{_address}/protected/sha256?message={input}");
            if (res.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(res.Data);
            }
        }
    }
}