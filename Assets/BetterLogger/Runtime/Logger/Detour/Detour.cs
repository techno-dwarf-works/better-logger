using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Better.Logger.Runtime
{
    [InitializeOnLoad]
    static class Detour
    {
        static void PatchInstanceMethod()
        {
            // get the MethodInfo for the method we're trying to patch
            // (it is private, so we need to dig around w/ reflection)

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            var extractformattedstacktrace = "ExtractFormattedStackTrace";
            var srcMethod
                = typeof(StackTraceUtility)
                    .GetMethod(extractformattedstacktrace,
                        bindingFlags); // make sure you use correct binding flags!

            var dstMethod =
                typeof(StackTraceOverrides)
                    .GetMethod(extractformattedstacktrace, bindingFlags);

            // patch the method function pointer
            DetourUtility.TryDetourFromTo(
                src: srcMethod,
                dst: dstMethod
            );

            var extractstringfromexceptioninternal = "ExtractStringFromExceptionInternal";
            var srcMethod1
                = typeof(StackTraceUtility)
                    .GetMethod(extractstringfromexceptioninternal, bindingFlags);

            var dstMethod1 =
                typeof(StackTraceOverrides)
                    .GetMethod(extractstringfromexceptioninternal, bindingFlags);

            // patch the method function pointer
            DetourUtility.TryDetourFromTo(
                src: srcMethod1,
                dst: dstMethod1
            );

            var extractStackTrace = "ExtractStackTrace";
            var srcMethod2
                = typeof(StackTraceUtility)
                    .GetMethod(extractStackTrace, bindingFlags);

            var dstMethod2 =
                typeof(StackTraceOverrides)
                    .GetMethod(extractStackTrace, bindingFlags);

            // patch the method function pointer
            DetourUtility.TryDetourFromTo(
                src: srcMethod2,
                dst: dstMethod2
            );
        }

        static Detour()
        {
            PatchInstanceMethod();
        }
    }
}