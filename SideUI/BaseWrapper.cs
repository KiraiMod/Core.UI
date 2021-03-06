using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI
{
    public class BaseWrapper
    {
        public BaseWrapper(Image Background, UIElement Element, Text Text)
        {
            this.Background = Background;
            this.Element = Element;
            this.Text = Text;
        }

        public Image Background;
        public Text Text;
        public UIElement Element;

        public virtual void OnSelected() { }
        public virtual void OnUnselected() { }
        public virtual void OnLeft() => Controller.OpenParent();
        public virtual void OnRight() { }
    }
}
