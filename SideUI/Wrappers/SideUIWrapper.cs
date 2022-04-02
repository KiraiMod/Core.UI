using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI.Wrappers
{
    public class SideUIWrapper : BaseWrapper
    {
        public new readonly UIElement<SideUI> Element;

        public SideUIWrapper(Image Background, UIElement<SideUI> Element, Text Text) : base(Background, Element, Text) => this.Element = Element;

        public override void OnRight()
        {
            Controller.current = Element.Bound._value;
            Controller.current.Sidebar.active = true;
            Controller.prevIndexes.Add(Controller.index);
            Controller.index = 0;
            Controller.previous = null;
            Controller.Redraw();
        }
    }
}
