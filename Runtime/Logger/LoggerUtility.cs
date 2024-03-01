using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Better.Extensions.Runtime;

namespace Better.Logger.Runtime
{
    public static class LoggerUtility
    {
        private static readonly string _firstCallerPattern = $@"(?<{ClassNameTag}>\w+)\.(?<{MethodNameTag}>\w+)\s*\(\s*\)";
        private static readonly string _lambdaNamePattern = @"<(.*?)>";
        private const string MethodNameFormat = "{0}{1}";
        private const string ClassNameTag = "ClassName";
        private const string MethodNameTag = "MethodName";
        private const string Unknown = "Unknown";

        public static string GetDeclaringTypeName(MethodBase methodBase)
        {
            if (methodBase == null)
            {
                return Unknown;
            }

            var declaringType = methodBase.DeclaringType;
            if (declaringType == null) return "Global";
            if (declaringType.IsAnonymous())
            {
                declaringType = declaringType.ReflectedType;
            }

            return declaringType != null ? declaringType.Name : "Global";
        }
        
        

        public static bool TryGetFirstCaller(Exception exception, out (string ClassName, string MethodName) info)
        {
            if (string.IsNullOrEmpty(exception.StackTrace))
            {
                info = new(Unknown, Unknown);
                return false;
            }

            // Use Regex to find matches in the input string
            var match = Regex.Match(exception.StackTrace, _firstCallerPattern);

            var classNameGroup = match.Groups[ClassNameTag];
            var methodNameGroup = match.Groups[MethodNameTag];
            if (!classNameGroup.Success || !methodNameGroup.Success)
            {
                info = new(Unknown, Unknown);
                return false;
            }

            var className = classNameGroup.Value;
            var methodName = methodNameGroup.Value;
            info = new(className, methodName);
            return true;
        }

        public static string GetMethodName(MethodBase methodBase)
        {
            if (methodBase == null)
            {
                return Unknown;
            }

            var name = ExtractMethodName(methodBase.Name);
            
            switch (name)
            {
                case ".ctor":
                case ".cctor":
                    var typeName = GetDeclaringTypeName(methodBase);
                    return string.Format(MethodNameFormat, typeName, name);
            }

            return name;
        }
        

        public static string ExtractMethodName(string methodName)
        {
            // Simplified regex pattern to match only within < and >
            var match = Regex.Match(methodName, _lambdaNamePattern);

            if (match.Success)
            {
                // Group 1 contains the method name
                return match.Groups[1].Value;
            }

            return methodName;
        }
    }
}