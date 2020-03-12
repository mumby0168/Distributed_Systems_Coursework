using System.Net.Http;

namespace DistSysACWClient
{
    public static class User
    {
        public static string Username { get; private set; }
        
        public static string ApiKey { get; private set; }

        public static void Set(string username, string apiKey)
        {
            Username = username;
            ApiKey = apiKey;
        }

        public static void Clear()
        {
            Username = string.Empty;
            ApiKey = string.Empty;
        }

        public static bool IsSet()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(ApiKey);
        }

        public static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
            return client;
        }

    }
}