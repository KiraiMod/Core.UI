namespace KiraiMod.Core.UI
{
    public abstract class AbstractUI
    {
        public AbstractUI(string ID) => UIManager.UIs[this.ID = ID] = this;
        public AbstractUI(string ID, string Name) : this(ID) => this.Name = Name;

        public readonly string ID;
        public readonly string Name;

        public Elements.InvokeElement AddElement(Elements.InvokeElement element) => (Elements.InvokeElement)AddElement((Elements.BaseElement)element);
        public Elements.BoundElement<T> AddElement<T>(Elements.BoundElement<T> element) => (Elements.BoundElement<T>)AddElement((Elements.BaseElement)element);
        protected abstract Elements.BaseElement AddElement(Elements.BaseElement element);

        public abstract bool RemoveElement(Elements.BaseElement element);
        public abstract bool RemoveElement(string element);
    }
}
