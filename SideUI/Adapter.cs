using KiraiMod.Core.ModuleAPI;
using System;

namespace KiraiMod.Core.UI.SideUI
{
    public static class Adapter
    {
        private static bool hasInitialized;

        public static void Initialize()
        {
            if (hasInitialized) return;
            hasInitialized = true;

            UIManager.HighGroups.ForEach(HandleHighGroup);
            UIManager.HighGroupAdded += HandleHighGroup;
        }

        private static void HandleHighGroup(UIGroup group) => HandleGroup(SideUI.GlobalContext, group);
        private static void HandleGroup(SideUI parent, UIGroup group)
        {
            SideUI ui = new(group.name);
            parent.AddElement(new UIElement<SideUI>(group.name, ui));
            ui.elements.ForEach(element => AddElement(ui, element));
            group.ElementAdded += element => AddElement(ui, element);
        }

        private static void AddElement(SideUI ui, UIElement element)
        {
            if (element is UIElement<UIGroup> subgroup)
                HandleGroup(ui, subgroup.Bound._value);
            else ui.AddElement(element);
        }
    }
}
