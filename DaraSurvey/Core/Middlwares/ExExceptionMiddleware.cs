using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DaraSurvey.Core
{
    public static class ExExceptionMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            bool isDevelopment = env.EnvironmentName == "Development";
            app.UseExceptionHandler(configure: cnfg =>
            {
                cnfg.Run(handler: async hdlr =>
                {
                    hdlr.Response.ContentType = "application/json";

                    var exception = hdlr.Features.Get<IExceptionHandlerFeature>();

                    if (exception != null)
                    {
                        // ServiceExceptionTypes
                        var serviceException = exception.Error as ServiceException;
                        if (serviceException != null)
                        {
                            hdlr.Response.StatusCode = (int)serviceException.StatusCode;
                            var handeledExceptionInfo = new HandeledExceptionInfo
                            {
                                ServiceExceptionCode = serviceException.ExceptionCode,
                                Messages = new string[] { $"{serviceException.ExceptionCode}" }
                            };
                            await hdlr.Response.WriteAsync(JsonConvert.SerializeObject(handeledExceptionInfo));
                        }
                        else
                        {
                            // UnhandeledExceptions
                            hdlr.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            var unHandeledExceptionInfo = new UnHandeledExceptionInfo();
                            unHandeledExceptionInfo.StatusCode = hdlr.Response.StatusCode;

                            if (isDevelopment)
                            {
                                unHandeledExceptionInfo.Message = exception.Error.GetAllMessages();
                                unHandeledExceptionInfo.StackTrace = exception.Error.StackTrace.ErrorSerialize();
                            }
                            else
                            {
                                unHandeledExceptionInfo.Message = "500 Internal Server Error";
                                unHandeledExceptionInfo.StackTrace = "500 Internal Server Error";
                            }

                            await hdlr.Response.WriteAsync(JsonConvert.SerializeObject(unHandeledExceptionInfo));
                        }
                    }
                });
            });
        }

        // --------------------

        public static string ErrorSerialize(this object errObj)
        {
            var seralizerConfigs = new JsonSerializerSettings
            {
                MaxDepth = 30,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var seralizedError = JsonConvert.SerializeObject(errObj, seralizerConfigs);

            return seralizedError;
        }

        // --------------------

        //private static void GenerateSerilog(HttpContext httpContext, IExceptionHandlerFeature contextFeature, ILogger<Startup> logger)
        //{
        //    string context = null;

        //    context = $"Request Path:{httpContext.Request.Path}{Environment.NewLine}";
        //    context = $"{context} Request QueryString: {httpContext.Request.QueryString}{Environment.NewLine}";
        //    context = $"{context} Request Body: {GetRequestBody(httpContext)}{Environment.NewLine}";

        //    context = $"{context} Error Message: {contextFeature.Error.Message}{Environment.NewLine}";
        //    context = $"{context} Error StackTrace: {contextFeature.Error.StackTrace.ErrorSerialize()}";

        //    logger.LogError(context);
        //}

        // --------------------

        public static string GetRequestBody(HttpContext httpContext)
        {
            try
            {
                var body = new StreamReader(httpContext.Request.Body);
                //The modelbinder has already read the stream and need to reset the stream index
                body.BaseStream.Seek(0, SeekOrigin.Begin);
                var requestBody = body.ReadToEnd();
                return requestBody;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    // --------------------

    public class UnHandeledExceptionInfo
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }

    // --------------------

    public class HandeledExceptionInfo
    {
        public ServiceExceptionCode ServiceExceptionCode { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}
