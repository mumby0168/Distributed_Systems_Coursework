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
        
    }
}