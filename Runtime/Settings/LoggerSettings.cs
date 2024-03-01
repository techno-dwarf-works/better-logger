using Better.ProjectSettings.Runtime;
using Better.Singletons.Runtime.Attributes;
using UnityEngine;

namespace Better.Logger.Runtime.Settings
{
    [ScriptableCreate(nameof(Better) + "/" + nameof(Logger))]
    public class LoggerSettings : ScriptableSettings<LoggerSettings>
    {
        [TextArea(1, 5)] 
        [SerializeField]
        private string _logFormat = $"[{LoggerDefinitions.ClassName}] {LoggerDefinitions.MethodName}: {LoggerDefinitions.Message}";
        [TextArea(1, 5)] 
        [SerializeField]
        private string _exceptionFormat = $"[{LoggerDefinitions.ClassName}] {LoggerDefinitions.MethodName}: {LoggerDefinitions.Message}";

        [SerializeField] 
        private bool _useFormatting = true;
        
        public string LogFormat => _logFormat;
        public string ExceptionFormat => _exceptionFormat;
        public bool UseFormatting => _useFormatting;
    }
}