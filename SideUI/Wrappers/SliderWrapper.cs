using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI.Wrappers
{
    public class SliderWrapper : BaseWrapper
    {
        private readonly UIElement<float> floatElem;

        public SliderWrapper(Image Background, UIElement<float> Element, Text text) : base(Background, Element, text)
        {
            floatElem = Element;
            floatElem.Bound.ValueChanged += value => Plugin.log.LogMessage(value);
        }

        public override void OnLeft() => floatElem.Bound.Value = floatElem.Bound._value - 0.1f;
        public override void OnRight() => floatElem.Bound.Value = floatElem.Bound._value + 0.1f;
    }
}
