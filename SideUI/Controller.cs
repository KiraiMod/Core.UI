using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KiraiMod.Core.UI.SideUI
{
    internal static class Controller
    {
        private static readonly Color highlight = new Color32(0x56, 0x00, 0xA5, 40);

        public static SideUI current;
        public static int index = 0;
        public static List<int> prevIndexes = new();
        public static bool locked = false;
        public static BaseWrapper previous;

        public static void Initialize(SideUI ui)
        {
            current = ui;

            Managers.KeybindManager.binds.Add("sideui.controller.up", new() { keys = new[] { Key.UpArrow }, OnClick = OnUp });
            Managers.KeybindManager.binds.Add("sideui.controller.down", new() { keys = new[] { Key.DownArrow }, OnClick = OnDown });
            Managers.KeybindManager.binds.Add("sideui.controller.left", new() { keys = new[] { Key.LeftArrow }, OnClick = OnLeft });
            Managers.KeybindManager.binds.Add("sideui.controller.right", new() { keys = new[] { Key.RightArrow }, OnClick = OnRight });
            Managers.KeybindManager.binds.Add("sideui.controller.back", new() { keys = new[] { Key.Backspace }, OnClick = OnBack });
        }

        public static void Redraw()
        {
            if (previous != null)
            {
                previous.Background.color = Color.black;
                previous.OnUnselected();
            }

            (previous = current.Wrappers[index]).Background.color = highlight;
            previous.OnSelected();
        }

        private static void OnUp()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                OpenParent();

            index--;
            if (index < 0)
                index = 0;

            Redraw();
        }

        private static void OnDown()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                OpenParent();

            index++;
            if (index >= current.Wrappers.Count)
                index = current.Wrappers.Count - 1;

            Redraw();
        }

        private static void OnLeft()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                OpenParent();

            current.Wrappers[index].OnLeft();
        }

        private static void OnRight() => current.Wrappers[index].OnRight();

        private static void OnBack()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                while (current.Parent != null)
                    OpenParent();
        }

        public static void OpenParent()
        {
            if (current.Parent != null)
            {
                current.Sidebar.active = false;
                current = current.Parent;
                index = prevIndexes.Pop();
                Redraw();
            }
        }
    }
}
