using System.Collections.Generic;
using Better.EditorTools.SettingsTools;
using Better.Logger.Runtime.Settings;
using Better.Tools.Runtime;
using UnityEditor;

namespace Better.Logger.EditorAddons.Settings
{
    internal class ValidationSettingProvider : ProjectSettingsProvider<LoggerSettings>
    {
        private readonly Editor _editor;

        public ValidationSettingProvider() : base(ProjectSettingsToolsContainer<LoggerSettingsTool>.Instance, SettingsScope.Project)
        {
            keywords = new HashSet<string>(new[] { "Better", "Logger", "Warnings", "Debug", "Error", "Exception", "Ignore", "Log" });
            _editor = Editor.CreateEditor(_settings);
        }

        [MenuItem(LoggerSettingsTool.MenuItemPrefix + "/" + BetterEditorDefines.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectSettingsToolsContainer<LoggerSettingsTool>.Instance.ProjectSettingKey);
        }

        protected override void DrawGUI()
        {
            _editor.OnInspectorGUI();
        }
    }
}