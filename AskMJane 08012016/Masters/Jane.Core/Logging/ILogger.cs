﻿using System;
using System.Web.Http.ExceptionHandling;

namespace Jane.Core.Logging
{
    public interface ILogger : IExceptionLogger
    {
        void Debug(string message);

        void DebugFormat(string message, params object[] args);

        void Warn(string message);

        void WarnFormat(string message, params object[] args);

        void Info(string message);

        void InfoFormat(string message, params object[] args);

        void Error(string message, Exception ex);

        void ErrorFormat(string message, params object[] args);
    }
}