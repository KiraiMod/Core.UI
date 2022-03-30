using System;

namespace KiraiMod.Core.UI.Elements
{
    public class InvokeElement : BaseElement
    {
        public InvokeElement(string Name) => this.Name = Name;

        public event Action OnClick;

        public void Click() => OnClick();
    }
}
