using KiraiMod.Core.UI.Elements;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI.Wrappers
{
    public class SideUIWrapper : BaseWrapper
    {
        private static BoundElement<SideUI> _element;

        public SideUIWrapper(Image Background, BoundElement<SideUI> Element, Text Text) : base(Background, Element, Text) => _element = Element;

        public override void OnRight()
        {
            Controller.current = _element.Bound._value;
            Controller.current.Sidebar.active = true;
            Controller.prevIndexes.Add(Controller.index);
            Controller.index = 0;
            Controller.Redraw();
        }
    }
}
