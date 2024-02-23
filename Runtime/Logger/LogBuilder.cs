using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Better.Logger.Runtime.Settings;
using UnityEngine;

namespace Better.Logger.Runtime
{
    public static class LogBuilder
    {
        private static readonly LoggerSettings Settings;
        private static readonly FieldInfo MessageField;
        
        private const string ExceptionMessageFieldName = "_message";
        private const string Null = "null";

        static LogBuilder()
        {
            Settings = Resources.Load<LoggerSettings>(nameof(LoggerSettings));
            MessageField = GetExceptionMessage();
        }

        private static string GetDeclaringTypeName(MethodBase methodBase)
        {
            return methodBase.DeclaringType != null ? methodBase.DeclaringType.Name : "Global";
        }

        private static string GetMethodName(MethodBase methodBase)
        {
            var name = methodBase.Name;
            switch (name)
            {
                case ".ctor":
                case ".cctor":
                    var typeName = GetDeclaringTypeName(methodBase);
                    const string format = "{0}{1}";
                    return string.Format(format, typeName, name);
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FieldInfo GetExceptionMessage()
        {
            var type = typeof(Exception);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            return type.GetField(ExceptionMessageFieldName, flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReplaceExceptionMessage(Exception exception, object message)
        {
            MessageField.SetValue(exception, message.ToString());
        }

        public static object BuildLogObject(object message)
        {
            if (!Settings.UseFormatting) 
                return message;

            return BuildLogByFormat(Settings.LogFormat, message, 3);
        }

        private static object BuildLogByFormat(string logFormat, object message, int jumpCount)
        {
            using (var stackFrame = new JumpStackFrame())
            {
                stackFrame.Jump(jumpCount);
                var methodBase = stackFrame.GetMethod();
                message ??= Null;
                return GetLogBuilder(logFormat, message.ToString(), methodBase);
            }
        }

        public static void BuildLogException(Exception exception, object message)
        {
            if (!Settings.UseFormatting) 
                return;

            var logObject = BuildLogByFormat(Settings.ExceptionFormat, message, 3);
            ReplaceExceptionMessage(exception, logObject);
        }
    }
}