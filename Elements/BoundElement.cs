using KiraiMod.Core.Utils;

namespace KiraiMod.Core.UI.Elements
{
    public class BoundElement<T> : BaseElement
    {
        public BoundElement(string Name) 
        {
            this.Name = Name;
            Bound = new();
        }

        public BoundElement(string Name, T value) : this(Name) => Bound._value = value;

        public BoundElement(string Name, Bound<T> bound)
        {
            this.Name = Name;
            Bound = bound;
        }

        public Bound<T> Bound;
    }
}
