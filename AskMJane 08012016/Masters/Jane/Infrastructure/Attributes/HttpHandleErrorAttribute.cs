using System;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Jane.Core.Logging;

namespace Jane.Web.Infrastructure.Attributes
{
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger = new Logger();

        public override void OnException(HttpActionExecutedContext context)
        {
            string page = string.Empty;
            if (context != null && context.Request != null)
            {
                if (context.Request.RequestUri != null)
                {
                    page = context.Request.RequestUri.ToString();
                }

                _logger.Error("UE: p:" + page, context.Exception);
            }
        }
    }
    public class HttpHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILogger _logger = new Logger();

        public override void OnException(ExceptionContext context)
        {
            var e = context.Exception;
            //context.Result = new EmptyResult();
            //if (!context.ExceptionHandled )
            //  return;

            LogException(context);
            base.OnException(context);
        }

        public void LogException(ExceptionContext ex)
        {
            string page = string.Empty;
            string cookies = string.Empty;
            string useragent = string.Empty;
            string ip = string.Empty;
            if (ex != null && ex.HttpContext != null && ex.HttpContext.Request != null)
            {
                if (ex.HttpContext.Request.Url != null)
                    page = ex.HttpContext.Request.Url.ToString();
                if (ex.HttpContext.Request.Headers != null && ex.HttpContext.Request.Headers["Cookie"] != null)
                {
                    try
                    {
                        cookies = ex.HttpContext.Request.Headers["Cookie"];
                    }
                    catch (Exception)
                    {
                        //Swallow is better that Spitting Exception
                    }
                }

                useragent = ex.HttpContext.Request.UserAgent;
                ip = ex.HttpContext.Request.UserHostAddress;
            }
            bool logError = !(useragent != null && useragent.Contains("Slackbot-LinkExpanding"));

            if (logError)
                _logger.Error("UE: p:" + page + " ip: " + ip + " ua:" + useragent + " c: " + cookies, ex.Exception);


        }
    }
}