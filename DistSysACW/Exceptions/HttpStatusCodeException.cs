using System;
using System.Net;

namespace DistSysACW.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCodeException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; }
    }
}