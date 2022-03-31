namespace KiraiMod.Core.UI.SideUI
{
    public static class Adapter
    {
        private static bool hasInitialized;

        public static void Initialize()
        {
            if (hasInitialized) return;
            hasInitialized = true;

            UIManager.HighGroupAdded += HandleGroup;
            UIManager.HighGroups.ForEach(HandleGroup);
        }

        private static void HandleGroup(UIGroup group)
        {
            SideUI ui = new(group.name);
            SideUI.GlobalContext.AddElement(new UIElement<SideUI>(group.name, ui));
            group.ElementAdded += element => ui.AddElement(element);
            ui.elements.ForEach(element => ui.AddElement(element));
        }
    }
}
