using System;
using System.Net;
using System.Net.Http;
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
            var client = User.CreateClient();
            var result = await client.GetResultAsStringAsync($"{_address}/protected/hello");
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(result.Data);
            }
        }
    }
}