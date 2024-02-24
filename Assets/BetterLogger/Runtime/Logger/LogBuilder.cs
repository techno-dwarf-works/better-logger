using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Better.Extensions.Runtime;
using Better.Logger.Runtime.Settings;
using UnityEngine;

namespace Better.Logger.Runtime
{
    public static class LogBuilder
    {
        private static readonly LoggerSettings _settings;
        private const int JumpCount = 4;
        private const string MethodNameFormat = "{0}{1}";
        private const string Null = "null";
        private const string Unknown = "Unknown";

        static LogBuilder()
        {
            _settings = Resources.Load<LoggerSettings>(nameof(LoggerSettings));
        }

        private static string GetDeclaringTypeName(MethodBase methodBase)
        {
            if (methodBase == null)
            {
                return Unknown;
            }

            return methodBase.DeclaringType != null ? methodBase.DeclaringType.Name : "Global";
        }

        private static string GetMethodName(MethodBase methodBase)
        {
            if (methodBase == null)
            {
                return Unknown;
            }

            var name = methodBase.Name;
            switch (name)
            {
                case ".ctor":
                case ".cctor":
                    var typeName = GetDeclaringTypeName(methodBase);
                    return string.Format(MethodNameFormat, typeName, name);
            }

            return name;
        }

        private static StringBuilder GetLogBuilder(string logFormat, string message, MethodBase methodBase)
        {
            var className = GetDeclaringTypeName(methodBase);
            var methodName = GetMethodName(methodBase);
            var length = logFormat.Length + className.Length + methodName.Length + message.Length;
            var builder = new StringBuilder(logFormat, length);

            builder.Replace(LoggerDefinitions.ClassName, className)
                .Replace(LoggerDefinitions.MethodName, methodName)
                .Replace(LoggerDefinitions.Message, message);
            return builder;
        }

        public static string BuildLogObject(string message)
        {
            if (!_settings.UseFormatting)
                return message;

            return BuildLogByFormat(_settings.LogFormat, message, JumpCount);
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

        public static void BuildLogException(Exception exception, string message)
        {
            if (!_settings.UseFormatting)
                return;

            var logObject = BuildLogByFormat(_settings.ExceptionFormat, message, JumpCount);
            exception.ReplaceExceptionMessageField(logObject);
        }
    }
}