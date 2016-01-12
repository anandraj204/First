using System;
using System.Diagnostics;
using System.Linq;
using log4net;
using log4net.Config;

namespace AskMJane.LeaflyScraper.Logging
{
    public class Logger : ILogger
    {
        private static ILog _logger;

        public Logger()
        {
            XmlConfigurator.Configure();
            _logger = GetLoggerInstance();
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
    }
}