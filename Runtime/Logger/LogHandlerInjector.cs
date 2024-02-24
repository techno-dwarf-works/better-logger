using UnityEditor;
using UnityEngine;

namespace Better.Logger.Runtime
{
    internal class LogHandlerInjector
    {
        private static readonly LogHandler _logHandler;

        static LogHandlerInjector()
        {
            _logHandler = new LogHandler();
        }

        [InitializeOnLoadMethod]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void OnInitialize()
        {
            Debug.unityLogger.logHandler = _logHandler;
        }
    }
}