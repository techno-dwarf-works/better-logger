using System.Collections.Generic;
using Better.Internal.Core.Runtime;
using Better.Logger.Runtime.Settings;
using Better.ProjectSettings.EditorAddons;
using UnityEditor;

namespace Better.Logger.EditorAddons.Settings
{
    internal class LoggerSettingProvider : DefaultProjectSettingsProvider<LoggerSettings>
    {
        private const string Path = PrefixConstants.BetterPrefix + "/" + nameof(Logger);
        
        public LoggerSettingProvider() : base(Path)
        {
            keywords = new HashSet<string>(new[] { "Better", "Logger", "Warnings", "Debug", "Error", "Exception", "Ignore", "Log" });
        }

        [MenuItem(Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + Path);
        }
    }
}