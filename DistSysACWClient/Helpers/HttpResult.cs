using System.Net;
using System.Net.Http;

namespace DistSysACWClient.Helpers
{
    public class HttpResult<T> : IHttpResult<T> where T : class
    {
        private HttpResult()
        {
            
        }
        private HttpResult(T data, HttpStatusCode statusCode, HttpResponseMessage rawResponse)
        {
            Data = data;
            StatusCode = statusCode;
            RawResponse = rawResponse;
        }
        public T Data { get; }
        public HttpStatusCode StatusCode { get; }
        public HttpResponseMessage RawResponse { get; }

        public static IHttpResult<T> Create(T data, HttpStatusCode code, HttpResponseMessage message)
        {
            return new HttpResult<T>(data, code, message);
        }
    }
}