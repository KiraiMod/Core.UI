using BepInEx;
using BepInEx.IL2CPP;

namespace KiraiMod.Core.UI
{
    [BepInPlugin(GUID, "KM.Core.GUI", "0.1.0")]
    public class Plugin : BasePlugin
    {
        const string GUID = "me.kiraihooks.KiraiMod.Core.UI";

        public override void Load() { }
    }
}
