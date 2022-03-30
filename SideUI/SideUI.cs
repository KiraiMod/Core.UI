using KiraiMod.Core.UI.Elements;
using KiraiMod.Core.UI.SideUI.Wrappers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI
{
    public class SideUI : AbstractUI
    {
        public static Lazy<SideUI> _globalContext = new(() =>
        {
            SideUI ui = new();
            Controller.Initialize(ui);
            return ui;
        });

        public static SideUI GlobalContext 
        {
            get => _globalContext.Value;
        }

        public List<BaseWrapper> Wrappers = new();

        public SideUI Parent;
        public GameObject Sidebar;
        public RectTransform rect;

        public SideUI(string ID) : base(ID, null) { }
        private SideUI() : base("global.SideUI", "SideUI")
        {
            Sidebar = ElementFactory.CreateSide(Name, UIManager.ScreenUI.transform.parent);

            rect = Sidebar.transform.Cast<RectTransform>();
            rect.sizeDelta = new(200, 0);
            rect.anchorMin = new(0, 1);
            rect.anchorMax = new(0, 1);
            rect.position = new(101, 1337.5f);
        }

        public void IncreaseSize()
        {
            rect.sizeDelta = rect.sizeDelta.AddY(25);
            rect.position = rect.position.AddY(-12.5f);
        }

        protected override BaseElement AddElement(BaseElement element)
        {
            Image image;
            Text text;
            BaseWrapper container;

            switch (element)
            {
                case BoundElement<bool> elemBool:
                    (image, text) = ElementFactory.CreateElement(Sidebar, elemBool.Name);
                    container = new ToggleWrapper(image, elemBool, text);
                    break;

                case BoundElement<SideUI> elemMenu:
                    if (elemMenu.Bound._value is null) return null;

                    (image, text) = ElementFactory.CreateElement(Sidebar, $"< {elemMenu.Name} >");

                    SideUI ui = elemMenu.Bound._value;
                    ui.Sidebar = ElementFactory.CreateSide(elemMenu.Name, Sidebar.transform);
                    ui.Parent = this;
                    ui.rect = ui.Sidebar.transform.Cast<RectTransform>();
                    ui.rect.sizeDelta = new(200, 0);
                    ui.rect.localPosition = new(201, 87.5f);
                    ui.Sidebar.active = false;

                    container = new SideUIWrapper(image, elemMenu, text);
                    break;

                default:
                    return null;
            }

            IncreaseSize();

            Wrappers.Add(container);
            if (Wrappers.Count == 1)
                Controller.Redraw();

            return element;
        }

        public override bool RemoveElement(BaseElement element)
        {
            throw new NotImplementedException();
        }

        public override bool RemoveElement(string element)
        {
            throw new NotImplementedException();
        }
    }
}
