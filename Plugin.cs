using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace KiraiMod.Core.UI
{
    [BepInPlugin(GUID, "KM.Core.GUI", "0.1.0")]
    [BepInDependency(Core.Plugin.GUID)]
    public class Plugin : BasePlugin
    {
        public const string GUID = "me.kiraihooks.KiraiMod.Core.UI";

        internal static ManualLogSource log;
        internal static ConfigFile cfg;

        public override void Load()
        {
            log = Log;
            cfg = Config;

            typeof(LegacyGUIManager).Initialize();
        }
    }
}
