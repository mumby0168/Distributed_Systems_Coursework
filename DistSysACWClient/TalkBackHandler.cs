using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DistSysACWClient.Helpers;

namespace DistSysACWClient
{
    public class TalkBackHandler
    {
        private Regex _arrayRegex;
        private readonly string _address;
        private readonly HttpClient _client;
        public TalkBackHandler(string address)
        {
            _address = address;
            _client = new HttpClient();
            _arrayRegex = new Regex(@"(\[([0-9]+,?)+])");
        }

        public async Task Hello()
        {
            Console.WriteLine("...please wait");
            var result = await _client.GetResultAsStringAsync($"{_address}/talkback/hello");
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(result.Data);
            }
            else if(result.StatusCode == HttpStatusCode.BadRequest)
            {
                Console.WriteLine(await result.RawResponse.Content.ReadAsStringAsync());
            }
        }

        public async Task Sort(string arrayToParse)
        {
            var array = _arrayRegex.Match(arrayToParse.Replace("TalkBack Sort", ""));
            if (array.Success)
            {
                var splitNumbers = array.Value.Replace("[", "").Replace("]", "").Split(',');
                var query = new StringBuilder();
                foreach (var num in splitNumbers)
                {
                    query.Append($"integers={num}&");
                }
                var result = await _client.GetResultAsStringAsync($"{_address}/talkback/sort/?{query}");
                if(result.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine(result.Data);
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                    Console.WriteLine(await result.RawResponse.Content.ReadAsStringAsync());

            }
            else
            {
                Console.WriteLine("Please enter a valid array to sort");
            }
        }
    }
}