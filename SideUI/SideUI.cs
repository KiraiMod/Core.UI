using KiraiMod.Core.UI.SideUI.Wrappers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI
{
    public class SideUI : UIGroup
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

        public SideUI(string name) : base(name) { }
        private SideUI() : base("SideUI")
        {
            Sidebar = ElementFactory.CreateSide(name, UIManager.ScreenUI.transform.parent);

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

        public override UIElement AddElement(UIElement element)
        {
            Image image;
            Text text;
            BaseWrapper container;

            switch (element)
            {
                case UIElement<bool> elemBool:
                    (image, text) = ElementFactory.CreateElement(Sidebar, elemBool.name);
                    container = new ToggleWrapper(image, elemBool, text);
                    break;

                case UIElement<float> elemFloat:
                    (image, text) = ElementFactory.CreateElement(Sidebar, $"[ {elemFloat.name} ]");
                    container = new SliderWrapper(image, elemFloat, text);
                    break;

                case UIElement<SideUI> elemMenu:
                    if (elemMenu.Bound._value is null) return null;

                    (image, text) = ElementFactory.CreateElement(Sidebar, $"< {elemMenu.name} >");

                    SideUI ui = elemMenu.Bound._value;
                    ui.Sidebar = ElementFactory.CreateSide(elemMenu.name, Sidebar.transform);
                    ui.Parent = this;
                    ui.rect = ui.Sidebar.transform.Cast<RectTransform>();
                    ui.rect.sizeDelta = new(200, 0);
                    ui.rect.localPosition = new(201, 12.5f);
                    ui.Sidebar.active = false;

                    container = new SideUIWrapper(image, elemMenu, text);
                    break;

                default:
                    return null;
            }

            IncreaseSize();

            base.AddElement(element);

            Wrappers.Add(container);
            if (Wrappers.Count == 1)
                Controller.Redraw();

            return element;
        }
    }
}
