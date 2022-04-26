using System;
using System.Net;

namespace DaraSurvey.Core
{
    public class ServiceException : Exception
    {
        public ServiceException(HttpStatusCode statusCode, ServiceExceptionCode exceptionCode, params string[] messages)
        {
            StatusCode = statusCode;
            ExceptionCode = exceptionCode;
            Messages = messages;
        }

        // ----------------------

        public ServiceException(HttpStatusCode statusCode, ServiceExceptionCode exceptionCode)
        {
            StatusCode = statusCode;
            ExceptionCode = exceptionCode;
        }

        // ----------------------

        public ServiceException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        // ----------------------

        public HttpStatusCode StatusCode { get; set; }
        public ServiceExceptionCode ExceptionCode { get; set; }
        public string[] Messages { get; set; }
    }
}
