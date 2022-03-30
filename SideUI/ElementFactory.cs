using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI
{
    internal static class ElementFactory
    {
        public static GameObject CreateSide(string Name, Transform parent)
        {
            GameObject side = new(Name);
            side.transform.SetParent(parent);
            side.AddComponent<Image>().color = Color.black;
            side.AddComponent<Outline>().effectColor = new Color32(0x56, 0x00, 0xA5, 0xFF);
            side.AddComponent<VerticalLayoutGroup>();
            side.AddComponent<LayoutElement>().ignoreLayout = true;

            return side;
        }

        public static (Image, Text) CreateElement(GameObject Sidebar, string name)
        {
            GameObject parent = new();

            Image image = parent.AddComponent<Image>();
            image.color = Color.black;

            parent.transform.SetParent(Sidebar.transform);
            parent.transform.Cast<RectTransform>().sizeDelta = new(200, 25);

            GameObject content = new();
            content.transform.SetParent(parent.transform);

            Text text = CreateText(content);
            text.text = name;

            return (image, text);
        }

        public static Text CreateText(GameObject go)
        {
            Text text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.resizeTextForBestFit = true;
            text.alignByGeometry = true;
            text.alignment = TextAnchor.MiddleCenter;

            go.transform.Cast<RectTransform>().sizeDelta = new(200, 25);

            return text;
        }
    }
}
