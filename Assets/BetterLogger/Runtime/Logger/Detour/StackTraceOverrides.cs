using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;
using Debug = UnityEngine.Debug;

namespace Better.Logger.Runtime
{
    internal static class StackTraceOverrides
    {
        internal static string ExtractFormattedStackTrace(StackTrace stackTrace)
        {
            try
            {
                return "MAGIC TRACE";
            }
            catch (Exception e)
            {
                return $"Failed to extract stacktrace:\n{stackTrace}\n Extractor error: \n{e}";
            }
        }

        private static string GetProjectFolder()
        {
            var folder = Path.GetDirectoryName(Application.dataPath);
            var field = typeof(StackTraceUtility).GetField("projectFolder", BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                var value = field.GetValue(null) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    folder = value;
                }
            }

            return folder;
        }
        
        [SecuritySafeCritical]
        public static string ExtractStackTrace()
        {
            return ExtractFormattedStackTrace(new StackTrace(1, true));
        }

        // Method used to extract the stack trace from an exception.
        internal static void ExtractStringFromExceptionInternal(object topLevel, out string message, out string stackTrace)
        {
            try
            {
                if (topLevel == null)
                    throw new ArgumentException("ExtractStringFromExceptionInternal called with null exception");
                if (!(topLevel is Exception exception))
                    throw new ArgumentException("ExtractStringFromExceptionInternal called with an exceptoin that was not of type System.Exception");
                StringBuilder stringBuilder = new StringBuilder(exception.StackTrace == null ? 512 : exception.StackTrace.Length * 2);
                message = "";
                string str1 = "";
                for (; exception != null; exception = exception.InnerException)
                {
                    str1 = str1.Length != 0 ? exception.StackTrace + "\n" + str1 : exception.StackTrace;
                    string str2 = exception.GetType().Name;
                    string str3 = "";
                    if (exception.Message != null)
                        str3 = exception.Message;
                    if (str3.Trim().Length != 0)
                        str2 = str2 + ": " + str3;
                    message = str2;
                    if (exception.InnerException != null)
                        str1 = "Rethrow as " + str2 + "\n" + str1;
                }

                stringBuilder.Append(str1 + "\n");
                StackTrace stackTrace1 = new StackTrace(1, true);
                stringBuilder.Append(ExtractFormattedStackTrace(stackTrace1));
                stackTrace = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                message = $"{topLevel}\n\nDemistifier: Unable to extract stack trace from exception: {(topLevel as Exception).GetType().Name}.";
                stackTrace = ex.ToString();
            }
        }
    }
}