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
        
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void OnInitialize()
        {
            Debug.unityLogger.logHandler = _logHandler;
        }
    }
}