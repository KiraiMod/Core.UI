using System.Collections.Generic;

namespace KiraiMod.Core.UI
{
    public static class UIManager
    {
        public static Dictionary<string, AbstractUI> UIs = new();

        public static UnityEngine.GameObject ScreenUI
        {
            get => global::KiraiMod.Managers.GUIManager.UserInterface;
        }
    }
}
