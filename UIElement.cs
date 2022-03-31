using KiraiMod.Core.Utils;
using System;

namespace KiraiMod.Core.UI
{
    public class UIElement 
    {
        public string name;

        public UIElement(string name) => this.name = name;

        public event Action Changed;

        public void Invoke() => Changed?.Invoke();
    }

    public class UIElement<T> : UIElement
    {
        public readonly Bound<T> Bound;

        public UIElement(string name, T value) : this(name) => Bound._value = value;
        public UIElement(string name) : base(name)
        {
            Bound = new();
            Bound.ValueChanged += val => Changed?.Invoke(val);
        }

        public UIElement(string name, Bound<T> bound) : base(name)
        {
            this.name = name;
            Bound = bound;
            Bound.ValueChanged += val => Changed?.Invoke(val);
        }

        public new event Action<T> Changed;

        [Obsolete("Change the inner Value of Bound")]
        public void Invoke(T val) => Changed?.Invoke(val);
    }
}
