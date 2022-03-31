using System;
using System.Collections.Generic;

namespace KiraiMod.Core.UI
{
    public static class UIManager
    {
        public static readonly List<UIGroup> HighGroups = new();
        public static event Action<UIGroup> HighGroupAdded;

        public static void RegisterHighGroup(UIGroup group)
        {
            HighGroups.Add(group);
            HighGroupAdded?.Invoke(group);
        }

        public static UnityEngine.GameObject ScreenUI
        {
            get => global::KiraiMod.Managers.GUIManager.UserInterface;
        }
    }
}
