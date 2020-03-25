using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreExtensions;
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

        public async Task StorePublicKey()
        {
            if (!User.IsSet())
            {
                Console.WriteLine(Constants.UserSetupFirst);
                return;
            }
            
            
            var client = User.CreateClient();

            var result = await client.GetAsync($"{_address}/protected/getpublickey");
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Got Public Key");
                User.PublicKey = await result.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Couldn’t Get the Public Key” if an error occurs");
            }
            
                
                
        }

        public async Task Sign(string input)
        {
            if (!User.IsSet())
            {
                Console.WriteLine(Constants.UserSetupFirst);
                return;
            }

            if (string.IsNullOrEmpty(User.PublicKey))
            {
                Console.WriteLine("Client doesn’t yet have the public key");
                return;
            }
            

            var client = User.CreateClient();
            var result = await client.GetAsync($"{_address}/protected/sign?message={input}");
            if (result.IsSuccessStatusCode)
            {
                var signed = await result.Content.ReadAsStringAsync();
                using (var provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlStringCore22(User.PublicKey);
                    byte[] data = signed.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
                    var res = provider.VerifyData(Encoding.ASCII.GetBytes(input), CryptoConfig.MapNameToOID("SHA1"), data);
                    if (res)
                    {
                        Console.WriteLine("Message was successfully signed");
                    }
                    else
                    {
                        Console.WriteLine("Message was not successfully signed");
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Message was not successfully signed");
            }
        }
    }
}