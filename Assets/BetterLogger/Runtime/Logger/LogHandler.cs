using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Logger.Runtime
{
    internal class LogHandler : ILogHandler
    {
        private readonly ILogHandler _defaultLogHandler;
        
        public LogHandler()
        {
            _defaultLogHandler = Debug.unityLogger.logHandler;
        }
        
        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            var newFormat = LogBuilder.BuildLogObject(format);
            _defaultLogHandler.LogFormat(logType, context, newFormat, args);
        }

        public void LogException(Exception exception, Object context)
        {
            LogBuilder.BuildLogException(exception, exception.Message);
            _defaultLogHandler.LogException(exception, context);
        }
    }
}