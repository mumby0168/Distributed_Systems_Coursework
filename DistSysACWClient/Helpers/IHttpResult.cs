using System.Net;
using System.Net.Http;

namespace DistSysACWClient.Helpers
{
    public interface IHttpResult<T> where T : class
    {
        T Data { get; }
        
        HttpStatusCode StatusCode { get; }
        
        HttpResponseMessage RawResponse { get; }
    }
}