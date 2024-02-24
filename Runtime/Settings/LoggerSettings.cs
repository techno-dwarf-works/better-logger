using Better.Tools.Runtime.Settings;
using UnityEngine;

namespace Better.Logger.Runtime.Settings
{
    public class LoggerSettings : ProjectSettings
    {
        [TextArea(1, 5)] [SerializeField]
        private string _logFormat = $"[{LoggerDefinitions.ClassName}] {LoggerDefinitions.MethodName}: {LoggerDefinitions.Message}";
        [TextArea(1, 5)] [SerializeField]
        private string _exceptionFormat = $"[{LoggerDefinitions.ClassName}] {LoggerDefinitions.MethodName}: {LoggerDefinitions.Message}";

        [SerializeField] 
        private bool _useFormatting = true;
        
        public string LogFormat => _logFormat;
        public string ExceptionFormat => _exceptionFormat;
        public bool UseFormatting => _useFormatting;
    }
}