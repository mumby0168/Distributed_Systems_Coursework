using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DistSysACWClient.Helpers
{
    public static class HttpExtensions
    {
        public static async Task<IHttpResult<T>> GetAsync<T>(this HttpClient client, string path) where T : class
        {
            var result = await client.GetAsync(path);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
                return HttpResult<T>.Create(data, result.StatusCode, result);
            }
            
            return HttpResult<T>.Create(null, result.StatusCode, result);
        }
        
        public static async Task<IHttpResult<string>> GetResultAsStringAsync(this HttpClient client, string path) 
        {
            var result = await client.GetAsync(path);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var str = await result.Content.ReadAsStringAsync();
                return HttpResult<string>.Create(str, result.StatusCode, result);
            }
            return HttpResult<string>.Create(null, result.StatusCode, result);
        }
    }
}