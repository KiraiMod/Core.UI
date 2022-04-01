using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace KiraiMod.Core.UI
{
    [BepInPlugin(GUID, "KM.Core.GUI", "0.1.0")]
    public class Plugin : BasePlugin
    {
        const string GUID = "me.kiraihooks.KiraiMod.Core.UI";

        internal static ManualLogSource log;

        public override void Load()
        {
            log = Log;

            SideUI.Adapter.Initialize();

            global::KiraiMod.Managers.GUIManager.OnLoad += ()=>{
            UIGroup g = new("Movement");
            g.RegisterAsHighest();
            g.AddElement("test", 5.0f);
            };
        }
    }
}
