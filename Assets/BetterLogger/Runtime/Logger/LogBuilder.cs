using System;
using System.Reflection;
using System.Text;
using Better.Commons.Runtime.Extensions;
using Better.Logger.Runtime.Settings;
using UnityEditor;
using UnityEngine;

namespace Better.Logger.Runtime
{
    public static class LogBuilder
    {
        private static LoggerSettings _settings;

        private const int JumpCount = 4;
        private const string Null = "null";

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            _settings = LoggerSettings.Instance;
        }

        public static string BuildLogObject(string message)
        {
            if (!_settings.UseFormatting)
                return message;

            return BuildLogByFormat(_settings.LogFormat, message, JumpCount);
        }

        public static void BuildLogException(Exception exception, string message)
        {
            if (!_settings.UseFormatting)
                return;
            
            string logObject;
            if (LoggerUtility.TryGetFirstCaller(exception, out var info))
            {
                logObject = GetLogBuilder(_settings.ExceptionFormat, message, info.ClassName, info.MethodName).ToString();
            }
            else
            {
                logObject = BuildLogByFormat(_settings.ExceptionFormat, message, JumpCount);
            }

            exception.ReplaceExceptionMessageField(logObject);
        }

        private static StringBuilder GetLogBuilder(string logFormat, string message, MethodBase methodBase)
        {
            var className = LoggerUtility.GetDeclaringTypeName(methodBase);
            var methodName = LoggerUtility.GetMethodName(methodBase);
            return GetLogBuilder(logFormat, message, className, methodName);
        }

        private static StringBuilder GetLogBuilder(string logFormat, string message, string className, string methodName)
        {
            var length = logFormat.Length + className.Length + methodName.Length + message.Length;
            var builder = new StringBuilder(logFormat, length);

            builder.Replace(LoggerDefinitions.ClassName, className)
                .Replace(LoggerDefinitions.MethodName, methodName)
                .Replace(LoggerDefinitions.Message, message);
            return builder;
        }

        private static string BuildLogByFormat(string logFormat, string message, int jumpCount)
        {
            using (var stackFrame = new JumpStackFrame())
            {
                stackFrame.Jump(jumpCount);
                var methodBase = stackFrame.GetMethod();
                message ??= Null;
                return GetLogBuilder(logFormat, message, methodBase).ToString();
            }
        }
    }
}