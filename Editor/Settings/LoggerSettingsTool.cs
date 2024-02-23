using Better.EditorTools.SettingsTools;
using Better.Logger.Runtime.Settings;
using Better.Tools.Runtime;

namespace Better.Logger.EditorAddons.Settings
{
    public class LoggerSettingsTool : ProjectSettingsTools<LoggerSettings>
    {
        private const string SettingMenuItem = nameof(Logger);
        public const string MenuItemPrefix = BetterEditorDefines.BetterPrefix + "/" + SettingMenuItem;

        public LoggerSettingsTool() : base(SettingMenuItem, SettingMenuItem)
        {
        }
    }
}