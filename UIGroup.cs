using KiraiMod.Core.Utils;
using System;
using System.Collections.Generic;

namespace KiraiMod.Core.UI
{
    public class UIGroup
    {
        public readonly List<UIElement> elements = new();
        public event Action<UIElement> ElementAdded;
        public event Action<UIElement> ElementRemoved;

        public readonly string name;

        public UIGroup(string name) => this.name = name;
        public UIGroup(string name, UIGroup parent) : this(name) => parent.AddElement(name, this);

        /// <summary> This will register your group as a top level group </summary>
        public virtual void RegisterAsHighest() => UIManager.RegisterHighGroup(this);

        // these overloads are for convenience and are not intended to be overloaded
        public UIElement<T> AddElement<T>(string name, T value) => (UIElement<T>)AddElement(new UIElement<T>(name, value));
        public UIElement<T> AddElement<T>(string name) => (UIElement<T>)AddElement(new UIElement<T>(name));

        public UIElement<T> AddElement<T>(string name, Bound<T> bound) => (UIElement<T>)AddElement(new UIElement<T>(name, bound));
        public UIElement<T> AddElement<T>(string name, MemoryEntry<T> entry)
        {
            var elem = (UIElement<T>)AddElement(new UIElement<T>(name, entry.Value));
            elem.Bound.ValueChanged += value => entry.Value = value;
            entry.ValueChanged += value => elem.Bound.Value = value;
            return elem;
        }

        public UIElement AddElement(string name) => AddElement(new UIElement(name));

        /// <remarks> Some implementations may require that the current group is already registered </remarks>
        public virtual UIElement AddElement(UIElement element)
        {
            elements.Add(element);
            ElementAdded?.Invoke(element);
            return element;
        }

        /// <returns> If the element was successfully removed </returns>
        public virtual bool RemoveElement(UIElement element) 
        {
            elements.Remove(element);
            ElementRemoved?.Invoke(element);
            return true;
        }
    }
}
