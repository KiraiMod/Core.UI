using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI.Wrappers
{
    public class SliderWrapper : BaseWrapper
    {
        private const string fmt = "0.00";

        private static readonly Color highlight = new Color32(32, 0, 69, 0xFF);

        private readonly UIElement<float> floatElem;
        private readonly GameObject repr;

        public SliderWrapper(Image Background, UIElement<float> Element, Text text) : base(Background, Element, text)
        {
            floatElem = Element;

            (Image reprBg, Text reprText) = ElementFactory.CreateElement(Background.gameObject, Element.Bound._value.ToString(fmt));
            reprBg.color = highlight;
            repr = reprBg.gameObject;
            repr.gameObject.active = false;

            RectTransform rect = repr.transform.Cast<RectTransform>();
            rect.sizeDelta = new(100, 25);
            rect.localPosition = new(150, 0);

            floatElem.Bound.ValueChanged += value => reprText.text = value.ToString(fmt);
        }

        public override void OnSelected() => repr.active = true;
        public override void OnUnselected() => repr.active = false;
        public override void OnLeft() => floatElem.Bound.Value = floatElem.Bound._value - GetAmount();
        public override void OnRight() => floatElem.Bound.Value = floatElem.Bound._value + GetAmount();

        private static float GetAmount() => Input.GetKey(KeyCode.LeftAlt) ? 0.1f : Input.GetKey(KeyCode.LeftShift) ? 10 : 1;
    }
}
