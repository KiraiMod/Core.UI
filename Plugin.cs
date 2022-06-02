using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace KiraiMod.Core.UI
{
    [BepInPlugin(GUID, "KM.Core.UI", "0.1.0")]
    [BepInDependency(Core.Plugin.GUID)]
    public class Plugin : BasePlugin
    {
        public const string GUID = "me.kiraihooks.KiraiMod.Core.UI";

        internal static ManualLogSource Logger;
        internal static ConfigFile Configuration;

        public override void Load()
        {
            Logger = Log;
            Configuration = Config;

            typeof(LegacyGUIManager).Initialize();
            Managers.ModuleManager.Register();
        }
    }
}
