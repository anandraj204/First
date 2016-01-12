using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using log4net;
using log4net.Config;

namespace Jane.Core.Logging
{
    public class Logger : ILogger
    {
        private static ILog _logger;

        public Logger()
        {
            XmlConfigurator.Configure();
            _logger = GetLoggerInstance();
        }

       public  void Log(ExceptionLoggerContext context)
       {
           var logger = GetLoggerInstance();
           if (context.Exception is DbEntityValidationException)
           {
               var exception = (DbEntityValidationException)context.Exception;
               _logger.Error(exception.EntityValidationErrors);
           }
           logger.Error(context.Exception);
       }

        public void Debug(string message)
        {
            var logger = GetLoggerInstance();
            logger.Debug(message);
        }

        public void DebugFormat(string message, params object[] args)
        {
            var logger = GetLoggerInstance();
            logger.DebugFormat(message, args);
        }

        public void Warn(string message)
        {
            var logger = GetLoggerInstance();
            logger.Warn(message);
        }

        public void WarnFormat(string message, params object[] args)
        {
            var logger = GetLoggerInstance();
            logger.WarnFormat(message, args);
        }

        public void Info(string message)
        {
            var logger = GetLoggerInstance();
            logger.Info(message);
        }

        public void InfoFormat(string message, params object[] args)
        {
            var logger = GetLoggerInstance();
            logger.InfoFormat(message, args);
        }

        public void Error(string message, Exception ex)
        {
            var logger = GetLoggerInstance();
            logger.Error(message, ex);
        }

        public void ErrorFormat(string message, params object[] args)
        {
            var logger = GetLoggerInstance();
            logger.ErrorFormat(message, args);
        }

        private static ILog GetLoggerInstance()
        {
            var callingClassType = (from frame in new StackTrace().GetFrames()
                                    let type = frame.GetMethod().DeclaringType
                                    where type != typeof(Logger)
                                    select type).First();
            if (callingClassType != null)
            {
                _logger = LogManager.GetLogger(callingClassType);
            }
            else
            {
                _logger = LogManager.GetLogger("wasNullType");
            }

            return _logger;
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (context.Exception is DbEntityValidationException)
                {
                    var exception = (DbEntityValidationException)context.Exception;
                    _logger.Error(exception.EntityValidationErrors);
                }
                _logger.Error(context.Exception);
            }, cancellationToken);
        }
    }
}