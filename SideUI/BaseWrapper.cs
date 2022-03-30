using KiraiMod.Core.UI.Elements;
using UnityEngine.UI;

namespace KiraiMod.Core.UI.SideUI
{
    public class BaseWrapper
    {
        public BaseWrapper(Image Background, BaseElement Element, Text Text)
        {
            this.Background = Background;
            this.Element = Element;
            this.Text = Text;
        }

        public Image Background;
        public Text Text;
        public BaseElement Element;

        public virtual void OnLeft() => Controller.OnBack();

        public virtual void OnRight() { }
    }
}
