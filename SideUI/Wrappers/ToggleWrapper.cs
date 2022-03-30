using KiraiMod.Core.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI.Wrappers
{
    public class ToggleWrapper : BaseWrapper
    {
        public static Color on = new Color32(0xCC, 0xCC, 0xFF, 0xFF);
        public static Color off = new Color32(0x56, 0x00, 0xA5, 0xFF);

        private readonly BoundElement<bool> boolElem;

        public ToggleWrapper(Image Background, BoundElement<bool> Element, Text text) : base(Background, Element, text)
        {
            boolElem = Element;
            boolElem.Bound.ValueChanged += value => text.color = value ? on : off;
            text.color = boolElem.Bound._value ? on : off;
        }

        public override void OnLeft()
        {
            if (!boolElem.Bound._value)
            {
                base.OnLeft();
                return;
            }

            boolElem.Bound.Value = false;
        }

        public override void OnRight() => boolElem.Bound.Value = true;
    }
}
