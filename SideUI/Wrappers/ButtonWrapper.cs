using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI.Wrappers
{
    public class ButtonWrapper : BaseWrapper
    {
        private readonly new UIElement Element;

        public ButtonWrapper(Image Background, UIElement Element, Text text) : base(Background, Element, text) => this.Element = Element;

        public override void OnRight() => Element.Invoke();
    }
}
